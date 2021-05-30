using Domain.Common;
using Domain.Common.Base;

namespace Domain.Entities
{
    public class Certificate : AuditableEntity
    {
        public string B64Certificate { get; set; }
        public string B64Password { get; set; }
        public int OwnerId { get; set; }
        public bool IsRevoked { get; set; }

        public virtual ApplicationUser Owner { get; set; }
    }
}