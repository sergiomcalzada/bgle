using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

using bgle.ComponentModel.DataAnnotations;

namespace bgle.CQRS.EntityFramework.Conventions
{
    public class StringUidConvention : Convention
    {
        public StringUidConvention()
        {
            this.Properties<string>()
                .Where(x => x.GetCustomAttributes(false).OfType<StringUidAttribute>().Any())
                .Configure(cfg => cfg.HasMaxLength(StringUidAttribute.MaxLenght).IsFixedLength().IsUnicode(false));
        }
    }
}