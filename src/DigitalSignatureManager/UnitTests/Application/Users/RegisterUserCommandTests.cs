using System.Linq;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Users.Commands.RegisterUser;
using Domain.Common;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UnitTests.Application.Base;

namespace UnitTests.Application.Users
{
    [TestFixture]
    public class RegisterUserCommandTests : BaseTest
    {
        private Mock<ILogger<RegisterUserCommand>> _logger;
        private Mock<IApplicationUserManager> _userManager;
        private DigitalSignatureManagerDbContext _context;

        private RegisterUserCommand _command;

        [SetUp]
        public new void SetUp()
        {
            _logger = new Mock<ILogger<RegisterUserCommand>>();
            _userManager = new Mock<IApplicationUserManager>(MockBehavior.Strict);
            _context = new DigitalSignatureManagerDbContext(ContextOptions);

            _userManager.Setup(x => x.AddToRole(It.IsAny<int>(), It.IsAny<Roles>())).Returns(Task.CompletedTask);
            _userManager.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new ApplicationUser {Id = 1});

            _command = new RegisterUserCommand(_userManager.Object, _logger.Object, _context);
        }

        [Test]
        public async Task Execute_ShouldCallUserManagerCreateUser()
        {
            var model = new RegisterUserModel
            {
                Email = "jon.doe@yahoo.com",
                Password = "SubparPassword"
            };

            await _command.Execute(model);

            _userManager.Verify(x => x.CreateUser("jon.doe@yahoo.com", "SubparPassword"), Times.Once);
        }

        [Test]
        public async Task Execute_ShouldReturnCreatedUserId()
        {
            var model = new RegisterUserModel
            {
                Email = "jon.doe@yahoo.com",
                Password = "SubparPassword"
            };

            var result = await _command.Execute(model);

            Assert.AreEqual(1, result.Id);
        }
    }
}