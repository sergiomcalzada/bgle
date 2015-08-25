using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using bgle.Contracts.Repository;
using bgle.EntityFramework.Conventions;

namespace bgle.Test.Domain.Context
{
    public class TestDomainContext : DbContext, IDbContext
    {
        public TestDomainContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public TestDomainContext()
            : this("TestDomainContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<StoreGeneratedIdentityKeyConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Conventions.Add<StringUidConvention>();
            modelBuilder.Conventions.Add<UnicodeConvention>();
            modelBuilder.Conventions.Add<UniqueConvention>();

            modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);
        }

        
    }
}