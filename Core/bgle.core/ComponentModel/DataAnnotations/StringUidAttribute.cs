using System;
using System.ComponentModel.DataAnnotations;

namespace bgle.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringUidAttribute : StringLengthAttribute
    {
        public const int MaxLenght = 13;

        public StringUidAttribute()
            : base(MaxLenght)
        {
            this.MinimumLength = MaxLenght;
        }
    }
}