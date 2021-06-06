using System;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.ErrorManagement.Exceptions;
using Application.Common.Models.DigitalSignatureModels;
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

        public async Task<InternalSignatureResponseModel> SignPdf(InternalSignatureRequestModel requestModel) =>
            await PostRequest<InternalSignatureResponseModel>(requestModel, "signature/pdf");

        public async Task<InternalSignatureResponseModel> SignBinary(InternalSignatureRequestModel requestModel) =>
            await PostRequest<InternalSignatureResponseModel>(requestModel, "signature/binary");

        public async Task<InternalVerificationResponseModel> VerifyPdf(BaseFileRequestModel requestModel) =>
            await PostRequest<InternalVerificationResponseModel>(requestModel, "verification/pdf");

        public async Task<InternalVerificationResponseModel> VerifyBinary(InternalDetachedSignatureRequestModel requestModel) =>
            await PostRequest<InternalVerificationResponseModel>(requestModel, "verification/binary");

        private Task<T> PostRequest<T>(object requestModel, string route)
        {
            var request = new RestRequest(route, DataFormat.Json);
            request.AddJsonBody(requestModel);

            // try
            // {
            return _restClient.PostAsync<T>(request);
            // }
            // catch (Exception e)
            // {
            //     throw new SignatureServiceException();
            // }
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