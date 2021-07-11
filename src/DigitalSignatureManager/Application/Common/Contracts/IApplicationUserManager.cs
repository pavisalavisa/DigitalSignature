using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Users.Commands.RegisterUser;
using Domain.Common;
using Domain.Enums;

namespace Application.Common.Contracts
{
    public interface IApplicationUserManager
    {
        Task<ApplicationUser> CreateUser(RegisterUserModel userModel);
        Task AddToRole(int userId, Roles role);
        Task<IEnumerable<Roles>> GetUserRoles(ApplicationUser user);
        Task<ApplicationUser> GetUser(string email, string password);
        Task<bool> EmailExists(string email);
    }
}