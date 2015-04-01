using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using EntityFramework.MappingAPI;
using EntityFramework.MappingAPI.Extensions;

namespace bgle.CQRS.EntityFramework.Bulk
{
    public class MappedDataReader<T> : IDataReader
        where T : class
    {
        private readonly IPropertyMap[] columns;

        private readonly IEnumerator<T> enumerator;

        private bool disposed;

        public IEnumerable<IPropertyMap> Columns { get { return this.columns.ToList(); } }

        public MappedDataReader(IEnumerable<T> entities, IEntityMap entityMap)
        {
            this.enumerator = entities.GetEnumerator();
            this.columns = entityMap.Properties.Where(x => !x.Computed && (!x.IsNavigationProperty || x.IsFk)).ToArray();
        }

        public bool Read()
        {
            var read = this.enumerator.MoveNext();
            if (read)
            {
                this.OnReaded(this.enumerator.Current);
            }
            return read;
        }

        public object GetValue(int i)
        {
            var current = this.enumerator.Current;
            if (current == null)
            {
                return null;
            }
            try
            {
                var col = this.columns[i];
                if (col.IsNavigationProperty)
                {
                    return 0;
                }

                var property = typeof(T).GetProperty(col.PropertyName);
                if (property == null || !property.CanRead)
                {
                    return null;
                }

                var value = property.GetValue(current);
                return value;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public bool IsDBNull(int i)
        {
            return GetValue(i) == null;
        }

        public string GetName(int i)
        {
            return this.columns[i].ColumnName;
        }

        public int GetOrdinal(string name)
        {
            return Array.FindIndex(this.columns, x => x.ColumnName == name);
        }

        public int FieldCount { get { return this.columns.Length; } }

        #region  Not Implemented

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        object IDataRecord.this[int i] { get { throw new NotImplementedException(); } }

        object IDataRecord.this[string name] { get { throw new NotImplementedException(); } }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public int Depth { get { return 0; } }

        public bool IsClosed { get { return false; } }

        public int RecordsAffected { get { return 0; } }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.enumerator.Dispose();
                }
            }
            this.disposed = true;
        }

        #endregion

        public event EventHandler<T> Readed;

        protected virtual void OnReaded(T e)
        {
            var handler = this.Readed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class SqlBulkProvider
    {
        protected readonly DbContext Context;

        private bool ownsConnection;

        private bool ownsTransaction;

        public SqlBulkCopyOptions Options { get; set; }

        public SqlBulkProvider(DbContext context)
        {
            this.Context = context;
            this.Options = SqlBulkCopyOptions.Default;
        }

        protected void RunInTransaction(Action<IDbTransaction> action)
        {
            this.EnsureConnection();
            var transaction = this.GetTransaction();

            try
            {
                action(transaction);
                if (this.ownsTransaction)
                {
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                if (this.ownsTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (this.ownsTransaction && transaction != null)
                {
                    transaction.Dispose();
                }
                this.CloseConnection();
            }
        }

        private void EnsureConnection()
        {
            if (this.Context.Database.Connection.State != ConnectionState.Open)
            {
                this.Context.Database.Connection.Open();
                this.ownsConnection = true;
            }
        }

        private void CloseConnection()
        {
            if (this.ownsConnection)
            {
                this.Context.Database.Connection.Close();
            }
        }

        private DbTransaction GetTransaction()
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;
            var entityTransaction = ((EntityConnection)objectContext.Connection).CurrentTransaction;
            if (entityTransaction != null)
            {
                return entityTransaction.StoreTransaction;
            }
            this.ownsTransaction = true;
            return this.Context.Database.BeginTransaction().UnderlyingTransaction;
        }
    }

    public class SqlBulkInsertProvider : SqlBulkProvider
    {
        public SqlBulkInsertProvider(DbContext context)
            : base(context)
        {
        }

        public void Run<T>(IEnumerable<T> entities) where T : class
        {
            this.RunInTransaction(transaction => this.Run(entities, transaction));
        }

        private void Run<T>(IEnumerable<T> entities, IDbTransaction transaction) where T : class
        {
            var sqlTransaction = transaction as SqlTransaction;
            if (sqlTransaction == null)
            {
                throw new NotSupportedException("Bulk updates are only supported on SQL Server");
            }
            this.Run(entities, sqlTransaction);
        }

        private void Run<T>(IEnumerable<T> entities, SqlTransaction transaction) where T : class
        {
            var map = this.Context.Db<T>();
            var tableName = string.Format("{0}.{1}", map.Schema, map.TableName);
            var insertIdentity = (SqlBulkCopyOptions.KeepIdentity & this.Options) > SqlBulkCopyOptions.Default;

            using (var mappedDataReader = new MappedDataReader<T>(entities, map))
            {
                using (var sqlBulkCopy = new SqlBulkCopy(transaction.Connection, this.Options, transaction) { DestinationTableName = tableName })
                {
                    foreach (var prop in mappedDataReader.Columns.Where(prop => !prop.IsIdentity || insertIdentity))
                    {
                        sqlBulkCopy.ColumnMappings.Add(prop.ColumnName, prop.ColumnName);
                    }
                    sqlBulkCopy.WriteToServer(mappedDataReader);
                }
            }
        }
    }

    public class SqlBulkUpdateProvider : SqlBulkProvider
    {
        public SqlBulkUpdateProvider(DbContext context)
            : base(context)
        {
        }

        public void Run<T>(IEnumerable<T> entities, Expression<Func<T, T>> updateExpression) where T : class
        {
            this.RunInTransaction(transaction => this.Run(entities, updateExpression, transaction));
        }

        private void Run<T>(IEnumerable<T> entities, Expression<Func<T, T>> updateExpression, IDbTransaction transaction) where T : class
        {
            var sqlTransaction = transaction as SqlTransaction;
            if (sqlTransaction == null)
            {
                throw new NotSupportedException("Bulk updates are only supported on SQL Server");
            }
            this.Run(entities, updateExpression, sqlTransaction);
        }

        private void Run<T>(IEnumerable<T> entities, Expression<Func<T, T>> updateExpression, SqlTransaction transaction) where T : class
        {
            var map = this.Context.Db<T>();
            var updateColumns = this.GetColumnsToUpdate(map, updateExpression);
            var columnsToInsert = this.GetColumnsToInsert(map, updateColumns);
            var temporaryTableName = string.Format("#{0}.{1}_{2}", map.Schema, map.TableName, DateTime.UtcNow.Ticks);


            using (var reader = new MappedDataReader<T>(entities, map))
            {
                reader.Readed += (sender, entity) => this.UpdateEntityWithExpressionValues(entity, updateExpression);

                //Create the bulk table
                this.CreateTempTable(temporaryTableName, columnsToInsert, transaction);

                //Insert the values in the bulk table
                this.InsertInTempTable(reader, temporaryTableName, columnsToInsert, transaction);

                //Update values of the origin table from the inserted values of the bulk table
                this.UpdateEntityTable(map, temporaryTableName, updateColumns, transaction);
            }
        }

        private void CreateTempTable(string temporaryTableName, IEnumerable<IPropertyMap> columnsToInsert, DbTransaction transaction)
        {
            var createTableCommand = this.Context.Database.Connection.CreateCommand();
            createTableCommand.CommandText = this.GetTemporaryTableCreateSQLScript(temporaryTableName, columnsToInsert);
            createTableCommand.Transaction = transaction;
            createTableCommand.ExecuteNonQuery();
        }

        private void InsertInTempTable(IDataReader reader, string temporaryTableName, IEnumerable<IPropertyMap> columnsToInsert, SqlTransaction transaction)
        {
            using (var bulkCopy = new SqlBulkCopy(transaction.Connection, this.Options, transaction) { DestinationTableName = temporaryTableName })
            {
                foreach (var c in columnsToInsert)
                {
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(c.ColumnName, c.ColumnName));
                }
                bulkCopy.WriteToServer(reader);
            }
        }

        private void UpdateEntityTable(IEntityMap map, string temporaryTableName, IEnumerable<IPropertyMap> updateColumns, DbTransaction transaction)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            var updateCommand = this.Context.Database.Connection.CreateCommand();
            updateCommand.CommandText = this.GetUpdateSql(map, temporaryTableName, updateColumns);
            updateCommand.Transaction = transaction;
            updateCommand.ExecuteNonQuery();
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

        private string GetTemporaryTableCreateSQLScript(string temporaryTableName, IEnumerable<IPropertyMap> insertColumns)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("CREATE TABLE [{0}] (", temporaryTableName);
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
                    case "SYSTEM.SINGLE":
                        builder.Append("single");
                        break;
                    case "SYSTEM.DOUBLE":
                        builder.Append("double");
                        break;
                    case "SYSTEM.DECIMAL":
                        builder.AppendFormat("decimal(18, 6)");
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

        private string GetUpdateSql(IEntityMap map, string temporaryTableName, IEnumerable<IPropertyMap> updateColumns)
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
                builder.AppendFormat("\n\t{0}.{2} = {1}.{2},", origin, temporaryTableName, column.ColumnName);
            }
            builder.Remove(builder.Length - 1, 1);
            builder.AppendFormat("\nFROM {0} INNER JOIN {1} ON", origin, temporaryTableName);
            for (var i = 0; i < map.Pks.Length; i++)
            {
                var key = map.Pks[i];
                builder.AppendFormat("\n\t{0}.{2} = {1}.{2}", origin, temporaryTableName, key.ColumnName);
                if (i < map.Pks.Length - 1)
                {
                    builder.Append(" AND");
                }
            }

            return builder.ToString();
        }

        //private void UpdateEntitiesWithExpressionValues<TEntity>(IEnumerable<TEntity> entities, Func<TEntity, TEntity> updateExpression, IEnumerable<IPropertyMap> updateColumns)

        //{
        //    var type = typeof(TEntity);
        //    var properties = updateColumns.Select(column => type.GetProperty(column.PropertyName)).ToArray();

        //    var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;
        //    var objectStateManager = objectContext.ObjectStateManager;

        //    foreach (var entity in entities)
        //    {
        //        var expressionItem = updateExpression(entity);
        //        //Update entity values
        //        foreach (var property in properties)
        //        {
        //            var value = property.GetValue(expressionItem, null);
        //            property.SetValue(entity, value, null);
        //        }

        //        //Update EF to use original values
        //        var stateEntry = objectStateManager.GetObjectStateEntry(entity);
        //        stateEntry.ApplyOriginalValues(entity);
        //        stateEntry.ChangeState(EntityState.Unchanged);
        //    }
        //}
    }
}