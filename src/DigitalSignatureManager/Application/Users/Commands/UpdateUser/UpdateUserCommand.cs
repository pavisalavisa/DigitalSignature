using System.Threading.Tasks;
using Application.Common.Base.Commands;
using Application.Common.Contracts;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : BaseUpdateCommand<ApplicationUser, UpdateUserModel>, IUpdateUserCommand
    {
        public UpdateUserCommand(IDigitalSignatureManagerDbContext context, ILogger<BaseUpdateCommand<ApplicationUser, UpdateUserModel>> logger) : base(context, logger)
        {
        }

        protected override Task ApplyChanges(ApplicationUser entity, UpdateUserModel model)
        {
            entity.Email = model.Email;
            entity.NormalizedEmail = model.Email.ToUpperInvariant();

            return Task.CompletedTask;
        }
    }
}