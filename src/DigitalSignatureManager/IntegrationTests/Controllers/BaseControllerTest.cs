using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api;
using Api.Authentication;
using IntegrationTests.Utilities;
using NUnit.Framework;

namespace IntegrationTests.Controllers
{
    public abstract class BaseControllerTest
    {
        protected HttpClient Client;
        private CustomWebApplicationFactory<Startup> _factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            Client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            Client.Dispose();
        }

        protected async Task AuthenticateAsAdmin()
        {
            var model = new UserAuthenticationInformation
            {
                Email = "kristicevic.antonio@gmail.com",
                Password = "Admin.123"
            };

            var response = await Client.PostAsync("/api/Token", JsonContent.Create(model));

            var jwt = await response.Content.ReadFromJsonAsync<JwtModel>();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Jwt);
        }
        
        protected async Task AuthenticateAsRegularUser()
        {
            var model = new UserAuthenticationInformation
            {
                Email = "jon.doe@gmail.com",
                Password = "Admin.123"
            };

            var response = await Client.PostAsync("/api/Token", JsonContent.Create(model));

            var jwt = await response.Content.ReadFromJsonAsync<JwtModel>();

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Jwt);
        }
    }
}