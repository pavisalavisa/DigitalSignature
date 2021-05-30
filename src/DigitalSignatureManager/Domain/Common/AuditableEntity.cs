using System;

namespace Domain.Common
{
    public class AuditableEntity : BaseEntity, IAuditableEntity

    {
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}