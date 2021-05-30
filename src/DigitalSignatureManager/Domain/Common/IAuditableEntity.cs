using System;

namespace Domain.Common
{
    public interface IAuditableEntity : IEntity
    {
        DateTime Created { get; set; }
        DateTime? Updated { get; set; }
    }
}