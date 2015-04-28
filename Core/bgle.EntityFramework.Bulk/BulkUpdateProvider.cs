using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using EntityFramework.MappingAPI;
using EntityFramework.MappingAPI.Extensions;

namespace bgle.EntityFramework.Bulk
{
    public abstract class BulkUpdateProvider<TTransaction> : BulkProvider
        where TTransaction : DbTransaction, IDbTransaction
    {
        protected BulkUpdateProvider(DbContext context)
            : base(context)
        {
        }

        public void Run<T>(IEnumerable<T> entities, Expression<Func<T, T>> updateExpression) where T : class
        {
            this.RunInTransaction(transaction => this.Run(entities, updateExpression, transaction));
        }

        private void Run<T>(IEnumerable<T> entities, Expression<Func<T, T>> updateExpression, IDbTransaction transaction) where T : class
        {
            var sqlTransaction = transaction as TTransaction;
            if (sqlTransaction == null)
            {
                throw new NotSupportedException(string.Format("Bulk updates are not supported in {0}", typeof(TTransaction).FullName));
            }
            this.Run(entities, updateExpression, sqlTransaction);
        }

        private void Run<T>(IEnumerable<T> entities, Expression<Func<T, T>> updateExpression, TTransaction transaction) where T : class
        {
            var map = this.Context.Db<T>();
            var updateColumns = this.GetColumnsToUpdate(map, updateExpression);
            var columnsToInsert = this.GetColumnsToInsert(map, updateColumns);
            var targetTableName = this.GetTemporaryTableName(map);

            using (var reader = new MappedDataReader<T>(entities, map))
            {
                reader.Readed += (sender, entity) => this.UpdateEntityWithExpressionValues(entity, updateExpression);

                //Create the bulk table
                this.CreateTargetTable(targetTableName, columnsToInsert, transaction);

                //Insert the values in the bulk table
                this.InsertInTempTable(reader, targetTableName, columnsToInsert, transaction);

                //Update values of the origin table from the inserted values of the bulk table
                this.UpdateEntityTable(map, targetTableName, updateColumns, transaction);

                this.DeleteTargetTable(targetTableName, transaction);
            }
        }

        protected abstract string GetTemporaryTableName(IEntityMap map);

        protected virtual void CreateTargetTable(string targetTableName, IEnumerable<IPropertyMap> columnsToInsert, DbTransaction transaction)
        {
            var createTableCommand = this.Context.Database.Connection.CreateCommand();
            createTableCommand.CommandText = this.GetTargetTableCreateSqlScript(targetTableName, columnsToInsert);
            createTableCommand.Transaction = transaction;
            createTableCommand.ExecuteNonQuery();
        }

        protected abstract void InsertInTempTable(IDataReader reader, string targetTableName, IEnumerable<IPropertyMap> columnsToInsert, TTransaction transaction);

        protected virtual void UpdateEntityTable(IEntityMap map, string targetTableName, IEnumerable<IPropertyMap> updateColumns, DbTransaction transaction)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            var updateCommand = this.Context.Database.Connection.CreateCommand();
            updateCommand.CommandText = this.GetUpdateSql(map, targetTableName, updateColumns);
            updateCommand.Transaction = transaction;
            updateCommand.ExecuteNonQuery();
        }

        protected virtual void DeleteTargetTable(string targetTableName, DbTransaction transaction)
        {
            var dropTableCommand = this.Context.Database.Connection.CreateCommand();
            dropTableCommand.CommandText = this.GetDropTableSqlScript(targetTableName);
            dropTableCommand.Transaction = transaction;
            dropTableCommand.ExecuteNonQuery();
        }

        private void UpdateEntityWithExpressionValues<T>(T entity, Expression<Func<T, T>> updateExpression) where T : class
        {
            var fun = updateExpression.Compile();
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;
            var objectStateManager = objectContext.ObjectStateManager;

            var expressionItem = fun(entity);
            var entry = objectStateManager.GetObjectStateEntry(entity);

            entry.ApplyCurrentValues(expressionItem);
            entry.AcceptChanges();
        }

        private IPropertyMap[] GetColumnsToUpdate<TEntity>(IEntityMap map, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            var memberInitExpression = updateExpression.Body as MemberInitExpression;
            if (memberInitExpression == null)
            {
                throw new ArgumentException(@"The update expression must be of type MemberInitExpression.", "updateExpression");
            }

            return
                memberInitExpression.Bindings.Where(binding => map.Pks.All(pk => pk.PropertyName != binding.Member.Name))
                    .Select(binding => map.Properties.FirstOrDefault(p => p.PropertyName == binding.Member.Name))
                    .Where(x => x != null)
                    .ToArray();
        }

        private IPropertyMap[] GetColumnsToInsert(IEntityMap map, IEnumerable<IPropertyMap> columns)
        {
            return map.Pks.Union(columns).ToArray();
        }

        protected virtual string GetTargetTableCreateSqlScript(string targetTableName, IEnumerable<IPropertyMap> insertColumns)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("CREATE TABLE [{0}] (", targetTableName);
            foreach (var column in insertColumns)
            {
                builder.AppendFormat("\n\t[{0}] ", column.ColumnName);
                switch (column.Type.GetEffectiveType().ToString().ToUpper())
                {
                    case "SYSTEM.INT16":
                        builder.Append("smallint");
                        break;
                    case "SYSTEM.INT32":
                        builder.Append("int");
                        break;
                    case "SYSTEM.INT64":
                        builder.Append("bigint");
                        break;
                    case "SYSTEM.DATETIME":
                        builder.Append("datetime");
                        break;
                    case "SYSTEM.SINGLE":
                        builder.Append("single");
                        break;
                    case "SYSTEM.DOUBLE":
                        builder.Append("double");
                        break;
                    case "SYSTEM.DECIMAL":
                        builder.AppendFormat("decimal(18, 6)");
                        break;
                    case "SYSTEM.STRING":
                        if (!column.FixedLength)
                        {
                            builder.AppendFormat("varchar({0})", column.MaxLength > 8000 ? "max" : column.MaxLength.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            builder.AppendFormat("nvarchar({0})", column.MaxLength > 4000 ? "max" : column.MaxLength.ToString(CultureInfo.InvariantCulture));
                        }

                        break;
                    default:
                        if (!column.FixedLength)
                        {
                            builder.AppendFormat("varchar({0})", column.MaxLength > 8000 ? "max" : column.MaxLength.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            builder.AppendFormat("nvarchar({0})", column.MaxLength > 4000 ? "max" : column.MaxLength.ToString(CultureInfo.InvariantCulture));
                        }
                        break;
                }
                if (column.IsRequired)
                {
                    builder.Append(" NOT NULL");
                }
                builder.Append(",");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append(")");

            return builder.ToString();
        }

        protected virtual string GetUpdateSql(IEntityMap map, string targetTableName, IEnumerable<IPropertyMap> updateColumns)
        {
            //@"UPDATE
            //    {origin}
            //SET
            //    {origin}.col1 = {target}.col1,
            //    {origin}.col2 = {target}.col2
            //FROM
            //    {origin}
            //INNER JOIN
            //    {target}
            //ON
            //    {origin}.pk1 = {target}.pk1";

            string origin = string.Format("[{0}].[{1}]", map.Schema, map.TableName);
            var builder = new StringBuilder();
            builder.AppendFormat("UPDATE {0} SET ", origin);
            foreach (var column in updateColumns)
            {
                builder.AppendFormat("\n\t{0}.{2} = {1}.{2},", origin, targetTableName, column.ColumnName);
            }
            builder.Remove(builder.Length - 1, 1);
            builder.AppendFormat("\nFROM {0} INNER JOIN {1} ON", origin, targetTableName);
            for (var i = 0; i < map.Pks.Length; i++)
            {
                var key = map.Pks[i];
                builder.AppendFormat("\n\t{0}.{2} = {1}.{2}", origin, targetTableName, key.ColumnName);
                if (i < map.Pks.Length - 1)
                {
                    builder.Append(" AND");
                }
            }

            return builder.ToString();
        }

        protected virtual string GetDropTableSqlScript(string targetTableName)
        {
            return string.Format("drop table {0}", targetTableName);
        }
    }
}