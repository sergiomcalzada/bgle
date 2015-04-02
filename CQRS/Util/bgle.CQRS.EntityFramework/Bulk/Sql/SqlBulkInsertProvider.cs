using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace bgle.CQRS.EntityFramework.Bulk.Sql
{
    public class SqlBulkInsertProvider : BulkInsertProvider<SqlTransaction>
    {
        public SqlBulkInsertProvider(DbContext context)
            : base(context)
        {
        }

        protected override void RunBulkInsert<T>(MappedDataReader<T> mappedDataReader, string tableName, SqlTransaction transaction)
        {
            var insertIdentity = (SqlBulkCopyOptions.KeepIdentity & this.Options) > SqlBulkCopyOptions.Default;
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