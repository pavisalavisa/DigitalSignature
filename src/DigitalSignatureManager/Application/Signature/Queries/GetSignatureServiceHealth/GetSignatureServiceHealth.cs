using System.Threading.Tasks;
using Application.Common.Contracts;
using Microsoft.Extensions.Logging;

namespace Application.Signature.Queries.GetSignatureServiceHealth
{
    public class GetSignatureServiceHealth : IGetSignatureServiceHealth
    {
        private readonly ILogger<GetSignatureServiceHealth> _logger;
        private readonly IDigitalSignatureService _digitalSignatureService;

        public GetSignatureServiceHealth(ILogger<GetSignatureServiceHealth> logger, IDigitalSignatureService digitalSignatureService)
        {
            _logger = logger;
            _digitalSignatureService = digitalSignatureService;
        }

        public async Task<bool> IsHealthy()
        {
            var isHealthy =  await _digitalSignatureService.IsHealthy();

            if (!isHealthy)
            {
                _logger.LogWarning("Digital signature service is not healthy.");
            }

            return isHealthy;
        }
    }
}