using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.Models;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRegisterUserCommand
    {
        private readonly IApplicationUserManager _userManager;
        private readonly ILogger<RegisterUserCommand> _logger;
        private readonly IDigitalSignatureManagerDbContext _context;

        public RegisterUserCommand(IApplicationUserManager userManager, ILogger<RegisterUserCommand> logger, IDigitalSignatureManagerDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public async Task<EntityCreatedModel> Execute(RegisterUserModel model)
        {
            _logger.LogInformation($"Creating user with email {model.Email}.");
            var user = await _userManager.CreateUser(model.Email, model.Password);
            await _userManager.AddToRole(user.Id, Roles.RegularUser);

            _logger.LogInformation($"User with email {model.Email} successfully created.");

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created a team for user {model.Email}.");

            return new EntityCreatedModel(user.Id);
        }
    }
}