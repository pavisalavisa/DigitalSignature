using System;
using System.Threading.Tasks;
using Api.Authentication;
using Application.Common.Contracts;
using Domain.Common;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace UnitTests.Api
{
    [TestFixture]
    public class JwtTokenProviderTests
    {
        private Mock<IApplicationUserManager> _userManager;
        private Mock<IOptionsSnapshot<JwtSettings>> _jwtSettings;

        private JwtTokenProvider _tokenProvider;

        [SetUp]
        public void SetUp()
        {
            _userManager = new Mock<IApplicationUserManager>(MockBehavior.Strict);
            _jwtSettings = new Mock<IOptionsSnapshot<JwtSettings>>(MockBehavior.Strict);

            _jwtSettings.Setup(x => x.Value).Returns(GetJwtSettings());

            _tokenProvider = new JwtTokenProvider(_userManager.Object, _jwtSettings.Object);
        }

        [Test]
        public void GenerateJwt_WithNullUser_ShouldThrowArgumentNullException()
        {
            var action = new AsyncTestDelegate(async () => await _tokenProvider.GenerateJwt(null));

            Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Test]
        public async Task GenerateJwt_WithProvidedUser_ShouldGenerateAppropriateToken()
        {
            var user = new ApplicationUser
            {
                UserName = "peterparker",
                Email = "peter.parker@dailybugle.com",
                Id = 109
            };

            _userManager.Setup(x => x.GetUserRoles(It.IsAny<ApplicationUser>())).ReturnsAsync(new[] {Roles.Admin});

            var result = await _tokenProvider.GenerateJwt(user);

            Assert.AreEqual("example.com", result.Audience);
            Assert.AreEqual("api.example.com", result.Issuer);
            Assert.GreaterOrEqual(result.ExpiresAt,DateTime.UtcNow);
            Assert.IsNotEmpty(result.Jwt);
        }

        private static JwtSettings GetJwtSettings() =>
            new()
            {
                Audience = "example.com",
                Issuer = "api.example.com",
                Secret = "MySuperbSecretMySuperbSecret",
                ExpirationInMinutes = 10
            };
    }
}