using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

using bgle.ComponentModel.DataAnnotations;

namespace bgle.CQRS.EntityFramework.Conventions
{
    public class UniqueConvention : Convention
    {
        public UniqueConvention()
        {
            this.Properties<string>()
                .Where(x => x.GetCustomAttributes(false).OfType<UniqueAttribute>().Any())
                .Configure(cfg => cfg.HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute { IsUnique = true })));
        }
    }
}