using Domain.Common.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Common
{
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
    }
}