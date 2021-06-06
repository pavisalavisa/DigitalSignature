using System.Linq;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.Models.DigitalSignatureModels;
using Microsoft.Extensions.Logging;

namespace Application.Verification.Commands.VerifyPdf
{
    public class VerifyPdfCommand : IVerifyPdfCommand
    {
        private readonly ILogger<VerifyPdfCommand> _logger;
        private readonly IDigitalSignatureService _digitalSignatureService;

        public VerifyPdfCommand(ILogger<VerifyPdfCommand> logger, IDigitalSignatureService digitalSignatureService)
        {
            _logger = logger;
            _digitalSignatureService = digitalSignatureService;
        }

        public async Task<VerifyPdfResponseModel> Execute(VerifyPdfModel model)
        {
            _logger.LogInformation($"Verifying file {model.FileName} for digital signatures.");

            var requestModel = CreateRequestModel(model);
            var response = await _digitalSignatureService.VerifyPdf(requestModel);

            return MapResponse(response);
        }

        private BaseFileRequestModel CreateRequestModel(VerifyPdfModel model) =>
            new()
            {
                B64Bytes = model.B64Bytes,
                FileName = model.FileName
            };

        private VerifyPdfResponseModel MapResponse(InternalVerificationResponseModel response) =>
            new()
            {
                DocumentName = response.DocumentFilename,
                SignaturesCount = response.SignaturesCount,
                ValidSignaturesCount = response.ValidSignaturesCount,
                Signatures = response.JaxbModel.SignatureOrTimestamp.Select(s => new VerifyPdfResponseModel.VerifyPdfSignatureModel
                {
                    Id = s.Id,
                    SignedBy = s.SignedBy,
                    SignatureTime = s.SigningTime,
                    SignatureFormat = s.SignatureFormat,
                    Result = s.Indication,
                    Errors = s.Errors,
                    Warnings = s.Warnings
                }).ToList()
            };
    }
}