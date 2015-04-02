using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

using EntityFramework.MappingAPI;

namespace bgle.CQRS.EntityFramework.Bulk.Sql
{
    public class SqlBulkUpdateProvider : BulkUpdateProvider<SqlTransaction>
    {
        public SqlBulkUpdateProvider(DbContext context)
            : base(context)
        {
        }

        protected override string GetTemporaryTableName(IEntityMap map)
        {
            return string.Format("#{0}.{1}_{2}", map.Schema, map.TableName, DateTime.UtcNow.Ticks);
        }

        protected override void InsertInTempTable(IDataReader reader, string targetTableName, IEnumerable<IPropertyMap> columnsToInsert, SqlTransaction transaction)
        {
            using (var bulkCopy = new SqlBulkCopy(transaction.Connection, this.Options, transaction) { DestinationTableName = targetTableName })
            {
                foreach (var c in columnsToInsert)
                {
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(c.ColumnName, c.ColumnName));
                }
                bulkCopy.WriteToServer(reader);
            }
        }
    }
}