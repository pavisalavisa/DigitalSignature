using Domain.Common.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Common
{
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }
    }
}