using System.Linq;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.Models.DigitalSignatureModels;
using Application.Verification.Commands.VerifyPdf;
using Microsoft.Extensions.Logging;

namespace Application.Verification.Commands.VerifyBinary
{
    public class VerifyBinaryCommand : IVerifyBinaryCommand
    {
        private readonly ILogger<VerifyPdfCommand> _logger;
        private readonly IDigitalSignatureService _digitalSignatureService;

        public VerifyBinaryCommand(ILogger<VerifyPdfCommand> logger, IDigitalSignatureService digitalSignatureService)
        {
            _logger = logger;
            _digitalSignatureService = digitalSignatureService;
        }

        public async Task<VerifyBinaryResponseModel> Execute(VerifyBinaryModel model)
        {
            _logger.LogInformation($"Verifying file {model.FileName} for digital signatures.");

            var requestModel = CreateRequestModel(model);
            var response = await _digitalSignatureService.VerifyBinary(requestModel);

            return MapResponse(response);
        }

        private InternalDetachedSignatureRequestModel CreateRequestModel(VerifyBinaryModel model) =>
            new()
            {
                B64Bytes = model.B64Bytes,
                FileName = model.FileName,
                B64XadesSignature = model.B64XadesSignature
            };

        private VerifyBinaryResponseModel MapResponse(InternalVerificationResponseModel response) =>
            new()
            {
                DocumentName = response.DocumentFilename,
                SignaturesCount = response.SignaturesCount,
                ValidSignaturesCount = response.ValidSignaturesCount,
                Signatures = response.JaxbModel.SignatureOrTimestamp.Select(s => new VerifyBinaryResponseModel.VerifyBinarySignatureModel
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