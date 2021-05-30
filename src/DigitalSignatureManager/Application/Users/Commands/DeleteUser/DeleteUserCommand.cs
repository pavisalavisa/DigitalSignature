using Application.Common.Base.Commands;
using Application.Common.Contracts;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : BaseDeleteCommand<ApplicationUser>, IDeleteUserCommand
    {
        public DeleteUserCommand(IDigitalSignatureManagerDbContext context, ILogger<BaseDeleteCommand<ApplicationUser>> logger) : base(context, logger)
        {
        }
    }
}