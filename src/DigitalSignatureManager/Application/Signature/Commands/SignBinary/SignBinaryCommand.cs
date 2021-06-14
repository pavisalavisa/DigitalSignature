﻿using System.Threading.Tasks;
using Application.Common;
using Application.Common.Contracts;
using Application.Common.Models.DigitalSignatureModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Signature.Commands.SignBinary
{
    public class SignBinaryCommand : ISignBinaryCommand
    {
        private readonly ILogger<SignBinaryCommand> _logger;
        private readonly IDigitalSignatureService _signatureService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICertificateService _certificateService;

        public SignBinaryCommand(IDigitalSignatureService signatureService, IHttpContextAccessor httpContextAccessor, ILogger<SignBinaryCommand> logger, ICertificateService certificateService)
        {
            _signatureService = signatureService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _certificateService = certificateService;
        }

        public async Task<SignedBinaryResponseModel> Execute(SignBinaryModel model)
        {
            var userId = _httpContextAccessor.GetUserIdFromClaims();

            _logger.LogInformation($"Signing pdf {model.FileName} for user with id {userId}.");

            var signatureModel = await CreateRequestModel(model, userId);

            var response = await _signatureService.SignBinary(signatureModel);

            return MapResponse(response);
        }

        private async Task<InternalSignatureRequestModel> CreateRequestModel(SignBinaryModel model, int userId) =>
            new()
            {
                B64Bytes = model.B64Bytes,
                FileName = model.FileName,
                Certificate = await _certificateService.GetUserCertificate(userId)
            };

        private SignedBinaryResponseModel MapResponse(InternalSignatureResponseModel response) =>
            new()
            {
                FileName = response.FileName,
                SignedB64Bytes = response.SignedB64Bytes
            };
    }
}