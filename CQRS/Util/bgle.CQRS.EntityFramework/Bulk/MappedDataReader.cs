using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using EntityFramework.MappingAPI;

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
}