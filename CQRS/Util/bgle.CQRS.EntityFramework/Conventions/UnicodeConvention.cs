using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

using bgle.ComponentModel.DataAnnotations;

namespace bgle.CQRS.EntityFramework.Conventions
{
    public class UnicodeConvention : Convention
    {
        public UnicodeConvention()
        {
            this.Properties<string>().Having(x => x.GetCustomAttributes(false).OfType<UnicodeAttribute>().FirstOrDefault()).Configure((cfg, att) => cfg.IsUnicode(att.Unicode));
        }
    }
}