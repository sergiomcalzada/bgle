using System;

namespace bgle.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UnicodeAttribute : Attribute
    {
        public bool Unicode { get; private set; }

        public UnicodeAttribute(bool unicode)
        {
            this.Unicode = unicode;
        }
    }
}