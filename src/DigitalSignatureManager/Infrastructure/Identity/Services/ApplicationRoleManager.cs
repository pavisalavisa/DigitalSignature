using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity.Services
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IApplicationRoleManager
    {
        private readonly ILogger<RoleManager<ApplicationRole>> _logger;

        public ApplicationRoleManager(IRoleStore<ApplicationRole> store,
            IEnumerable<IRoleValidator<ApplicationRole>> roleValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationRole>> logger) : base(store, roleValidators,
            keyNormalizer, errors, logger)
        {
            _logger = logger;
        }

        public async Task CreateRoleAsync(Roles role)
        {
            var applicationRole = new ApplicationRole
            {
                Name = role.ToString()
            };
            
            var result = await base.CreateAsync(applicationRole);

            if (!result.Succeeded)
                _logger.LogError(
                    $"Something went wrong while creating role {role.ToString()}: {string.Join("", result.Errors.Select(e => $"{e.Code}:{e.Description}"))}");
        }
    }
}