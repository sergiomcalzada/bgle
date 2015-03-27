using bgle.ComponentModel.DataAnnotations;

namespace bgle.DomainEntity
{
    public class DomainEntity : Entity<string>
    {
        [StringUid]
        public override string Id { get; set; }
        
    }
}