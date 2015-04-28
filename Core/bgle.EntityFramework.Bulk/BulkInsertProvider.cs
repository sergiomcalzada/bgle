using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;

using EntityFramework.MappingAPI.Extensions;

namespace bgle.EntityFramework.Bulk
{
    public abstract class BulkInsertProvider<TTransaction> : BulkProvider
        where TTransaction : class, IDbTransaction
    {
        protected BulkInsertProvider(DbContext context)
            : base(context)
        {
        }

        public void Run<T>(IEnumerable<T> entities) where T : class
        {
            this.RunInTransaction(transaction => this.Run(entities, transaction));
        }

        private void Run<T>(IEnumerable<T> entities, IDbTransaction transaction) where T : class
        {
            var sqlTransaction = transaction as TTransaction;
            if (sqlTransaction == null)
            {
                throw new NotSupportedException(string.Format("Bulk inserts are not supported in {0}", typeof(TTransaction).FullName));
            }
            this.Run(entities, sqlTransaction);
        }

        private void Run<T>(IEnumerable<T> entities, TTransaction transaction) where T : class
        {
            var map = this.Context.Db<T>();
            var tableName = string.Format("{0}.{1}", map.Schema, map.TableName);

            using (var mappedDataReader = new MappedDataReader<T>(entities, map))
            {
                this.RunBulkInsert(mappedDataReader, tableName, transaction);
            }
        }

        protected abstract void RunBulkInsert<T>(MappedDataReader<T> mappedDataReader, string tableName, TTransaction transaction) where T : class;
    }
}