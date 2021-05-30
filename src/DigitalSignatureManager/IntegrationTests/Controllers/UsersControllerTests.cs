using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.UpdateUser;
using NUnit.Framework;

namespace IntegrationTests.Controllers
{
    [TestFixture]
    public class UsersControllerTests : BaseControllerTest
    {
        [Test]
        public async Task PostRegistration_WithCorrectModel_ShouldReturnCreated()
        {
            var model = new RegisterUserModel {Email = "jane.doe@gmail.com", Password = "superHardPassword123."};
            var response = await Client.PostAsync("/api/Users/Registration", JsonContent.Create(model));

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.NotZero((await response.Content.ReadFromJsonAsync<EntityCreatedModel>()).Id); // no users in db
        }

        [Test]
        public async Task PostRegistration_WithWeakPassword_ShouldReturnBadRequest()
        {
            var model = new RegisterUserModel {Email = "jamal.doe@gmail.com", Password = "easyPassword"};
            var response = await Client.PostAsync("/api/Users/Registration", JsonContent.Create(model));

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task PostRegistration_WithAlreadyTakenEmail_ShouldReturnBadRequest()
        {
            var firstUser = new RegisterUserModel {Email = "jon.doe@gmail.com", Password = "superHardPassword123."};
            await Client.PostAsync("/api/Users/Registration", JsonContent.Create(firstUser));

            var secondUser = new RegisterUserModel {Email = "jon.doe@gmail.com", Password = "MyPassword1239!"};

            var response = await Client.PostAsync("/api/Users/Registration", JsonContent.Create(secondUser));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnPaginatedResult()
        {
            await AuthenticateAsAdmin();

            var response = await Client.GetAsync("/api/Users");

            var responseContent = await response.Content.ReadFromJsonAsync<PagingResultModel<IdNameModel>>();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(3, responseContent.Items.Count()); // 3 seeded users
        }

        [Test]
        public async Task GetSingleUsers_WithNonExistentId_ShouldReturnNotFound()
        {
            await AuthenticateAsAdmin();

            var response = await Client.GetAsync("/api/Users/112");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Put_ShouldReturnOk()
        {
            await AuthenticateAsAdmin();

            var model = new UpdateUserModel
            {
                Email = "frank.sinatra@google.com"
            };

            var response = await Client.PutAsync("/api/Users/1", JsonContent.Create(model));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Put_WithNonexistentUser_ShouldReturnBadRequest()
        {
            await AuthenticateAsAdmin();

            var model = new UpdateUserModel
            {
                Email = "frank.sinatra@google.com"
            };

            var response = await Client.PutAsync("/api/Users/12", JsonContent.Create(model));

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task DeleteUser_ShouldReturnNoContent()
        {
            await AuthenticateAsAdmin();
            var response = await Client.DeleteAsync("api/Users/1");

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}