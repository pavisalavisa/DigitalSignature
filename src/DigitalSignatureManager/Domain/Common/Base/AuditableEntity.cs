using System;

namespace Domain.Common.Base
{
    public class AuditableEntity : BaseEntity, IAuditableEntity
    {
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}