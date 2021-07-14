using Application.Common.Contracts;
using Application.Users.Commands.RegisterUser;
using Moq;
using NUnit.Framework;

namespace UnitTests.Application.Users
{
    [TestFixture]
    public class RegisterUserModelValidatorTests
    {
        private Mock<IApplicationUserManager> _userManager;
        private RegisterUserModel.RegisterUserModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _userManager = new Mock<IApplicationUserManager>(MockBehavior.Strict);

            _userManager.Setup(x => x.EmailExists(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(false);

            _validator = new RegisterUserModel.RegisterUserModelValidator(_userManager.Object);
        }

        [Test]
        public void GivenEmptyEmailAddress_Validate_ShouldHaveAValidationError()
        {
            var model = new RegisterUserModel {Email = null, Password = "SuperbPassword.123"};

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
        }

        [Test]
        public void GivenInvalidEmailAddress_Validate_ShouldHaveAValidationError()
        {
            var model = new RegisterUserModel {Email = "colin.fgmail", Password = "SuperbPassword.123"};

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
        }

        [Test]
        public void GivenExistingEmailAddress_Validate_ShouldHaveAValidationError()
        {
            var model = new RegisterUserModel {Email = "colin.f@gmail.com", Password = "SuperbPassword.123"};

            _userManager.Setup(x => x.EmailExists("colin.f@gmail.com", null)).ReturnsAsync(true);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
        }

        [TestCase("")]
        [TestCase("1234")]
        [TestCase("admin1234")]
        [TestCase("Password!")]
        public void GivenWeakPassword_Validate_ShouldHaveAValidationError(string password)
        {
            var model = new RegisterUserModel {Email = "colin.f@gmail.com", Password = password};

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.GreaterOrEqual(result.Errors.Count, 1);
        }
    }
}