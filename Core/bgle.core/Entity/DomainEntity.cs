using bgle.ComponentModel.DataAnnotations;

namespace bgle.Entity
{
    public class DomainEntity : Entity<string>
    {
        [StringUid]
        public override string Id { get; set; }
    }
}