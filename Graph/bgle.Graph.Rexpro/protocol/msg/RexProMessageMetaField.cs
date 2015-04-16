using System;

namespace bgle.Graph.Rexpro.Core.protocol.msg
{
    public class RexProMessageMetaField
    {
        protected String Key;
        protected Boolean Required;
        protected Object DefaultValue;
        protected Type FieldType;

        public RexProMessageMetaField(string key, bool required, object defaultValue, Type fieldType)
        {
            this.Key = key;
            this.Required = required;
            this.DefaultValue = defaultValue;
            this.FieldType = fieldType;
        }

        public void ValidateMeta(RexProMessageMeta meta)
        {
            //handle missing / null values
            if (meta[this.Key] == null)
            {
                if (this.DefaultValue != null)
                {
                    meta.Add(this.Key, this.DefaultValue);
                    return;
                }
                if (this.Required)
                {
                    throw new RexProException("meta value is required for " + this.Key);
                }
                //otherwise, bail out
                return;
            }

            //handle improperly typed values
            var val = meta[this.Key];
            if (!this.FieldType.IsInstanceOfType(val))
            {
                throw new RexProException(this.FieldType + " type required for " + this.Key + ", " + val.GetType() + " found");
            }

        }
    }
}