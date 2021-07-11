using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Users.Commands.RegisterUser;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Identity.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>, IApplicationUserManager
    {
        private readonly ILogger<UserManager<ApplicationUser>> _logger;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
                services, logger)
        {
            _logger = logger;
        }

        public async Task<ApplicationUser> CreateUser(RegisterUserModel userModel)
        {
            var user = new ApplicationUser
            {
                UserName = userModel.Email,
                Email = userModel.Email, 
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                OrganizationName = userModel.OrganizationName
            };

            var result = await base.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
                throw new ApplicationException(
                    $"Something went wrong while creating user {userModel.Email}: {string.Join(",", result.Errors.Select(e => $"{e.Code}:{e.Description}"))}");

            return user;
        }

        public async Task AddToRole(int userId, Roles role)
        {
            var user = await base.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new ApplicationException($"User with id {userId} does not exist.");

            var result = await base.AddToRoleAsync(user, role.ToString());

            if (!result.Succeeded)
                throw new ApplicationException(
                    $"Something went wrong while adding role {role} to user {user.Email}: {string.Join(",", result.Errors.Select(e => $"{e.Code}:{e.Description}"))}");
        }

        public async Task<IEnumerable<Roles>> GetUserRoles(ApplicationUser user)
        {
            var roles = await base.GetRolesAsync(user);

            return roles.Select(r => Enum.Parse<Roles>(r, true)).ToList();
        }

        public async Task<ApplicationUser> GetUser(string email, string password)
        {
            var user = await base.Users.FirstOrDefaultAsync(usr => usr.UserName == email);
            if (user is null)
                return null;

            var isPasswordValid = await base.CheckPasswordAsync(user, password);

            return !isPasswordValid ? null : user;
        }

        public Task<bool> EmailExists(string email)
        {
            return base.Users.AnyAsync(u => u.Email == email);
        }
    }
}