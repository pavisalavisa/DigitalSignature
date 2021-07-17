using System.Threading.Tasks;
using Application.Common;
using Application.Common.Contracts;
using Application.Common.ErrorManagement.Exceptions;
using Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.UpdatePersonalInformation
{
    public class UpdatePersonalInformationCommand : IUpdatePersonalInformationCommand
    {
        private readonly ILogger<UpdatePersonalInformationCommand> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDigitalSignatureManagerDbContext _context;

        public UpdatePersonalInformationCommand(IHttpContextAccessor httpContextAccessor,
            IDigitalSignatureManagerDbContext context, ILogger<UpdatePersonalInformationCommand> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;
        }

        public async Task Execute(UpdatePersonalInformationModel model)
        {
            var userId = _httpContextAccessor.GetUserIdFromClaims();

            _logger.LogInformation("Updating personal information for user with id {Id}", userId);
            
            var dbUser = await _context.EntitySet<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            if (dbUser is null)
            {
                throw new NotFoundException(nameof(ApplicationUser), userId);
            }

            ApplyChanges(dbUser, model);

            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Personal information for user with id {Id} was successfully updated.", userId);
        }

        private static void ApplyChanges(ApplicationUser dbUser, UpdatePersonalInformationModel model)
        {
            dbUser.FirstName = model.FirstName;
            dbUser.LastName = model.LastName;
            dbUser.OrganizationName = model.OrganizationName;
            dbUser.Email = model.Email;
        }
    }
}