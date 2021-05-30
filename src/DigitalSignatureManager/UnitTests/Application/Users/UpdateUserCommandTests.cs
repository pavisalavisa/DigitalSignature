using System.Collections.Generic;
using Application.Users.Commands.UpdateUser;
using Domain.Common;
using NUnit.Framework;
using UnitTests.Application.Base;

namespace UnitTests.Application.Users
{
    [TestFixture]
    public class UpdateUserCommandTests : BaseUpdateCommandTests<ApplicationUser,UpdateUserModel,UpdateUserCommand>
    {
        protected override IEnumerable<ApplicationUser> GetSeedEntities()
        {
            yield return new ApplicationUser {Email = "firo.laslo@gmail.com"};
        }

        protected override UpdateUserCommand ConstructCommand() => new(_context, _logger.Object);

        protected override UpdateUserModel GetValidModel() =>
            new()
            {
                Email = "jon.doevski@gmail.com"
            };

        protected override void UpdateAssertions(ApplicationUser updatedEntity)
        {
            Assert.AreEqual("jon.doevski@gmail.com",updatedEntity.Email);
        }
    }
}