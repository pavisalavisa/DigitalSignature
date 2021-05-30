using System;
using Application.Users.Commands.DeleteUser;
using Domain.Common;
using NUnit.Framework;
using UnitTests.Application.Base;

namespace UnitTests.Application.Users
{
    
    [TestFixture]
    public class DeleteTeamCommandTests : BaseDeleteCommandTests<ApplicationUser, DeleteUserCommand>
    {
        protected override DeleteUserCommand ConstructCommand() => new(_context, _logger.Object);

        protected override ApplicationUser GetValidEntity()
        {
            return new()
            {
                Email = "jon.doe@google.com",
                ConcurrencyStamp = Guid.NewGuid().ToString("N")
            };
        }
    }
}