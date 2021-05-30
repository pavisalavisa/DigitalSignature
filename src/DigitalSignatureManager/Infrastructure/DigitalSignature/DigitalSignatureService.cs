using System;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.ErrorManagement.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Infrastructure.DigitalSignature
{
    public class DigitalSignatureService : IDigitalSignatureService
    {
        private readonly ILogger<DigitalSignatureService> _logger;
        private readonly IRestClient _restClient;

        public DigitalSignatureService(ILogger<DigitalSignatureService> logger, IRestClient restClient, IConfiguration configuration)
        {
            _logger = logger;
            _restClient = restClient;

            var digitalSignatureServiceBaseUri = configuration.GetSection("DigitalSignatureServiceBaseUri").Value;
            if (digitalSignatureServiceBaseUri is null)
            {
                throw new ConfigurationException("DigitalSignatureServiceBaseUri");
            }

            _restClient.BaseUrl = new Uri(digitalSignatureServiceBaseUri);
        }

        public async Task SignPdf()
        {

            // var response = await _restClient.ExecutePostAsync<>();

            throw new System.NotImplementedException();
        }

        public Task SignBinary()
        {
            throw new System.NotImplementedException();
        }

        public Task VerifyPdf()
        {
            throw new System.NotImplementedException();
        }

        public Task VerifyBinary()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsHealthy()
        {
            try
            {
                var response = await _restClient.ExecuteGetAsync(new RestRequest("ping"));

                if (response.Content == "PONG")
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened while checking the signature service status.");
            }

            return false;
        }
    }
}