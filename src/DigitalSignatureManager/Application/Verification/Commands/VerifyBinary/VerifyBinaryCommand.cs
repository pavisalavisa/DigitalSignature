using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Contracts;
using Application.Common.Models.DigitalSignatureModels;
using Application.Verification.Commands.VerifyPdf;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Verification.Commands.VerifyBinary
{
    public class VerifyBinaryCommand : IVerifyBinaryCommand
    {
        private readonly ILogger<VerifyPdfCommand> _logger;
        private readonly IDigitalSignatureService _digitalSignatureService;
        private readonly IEventService _eventService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VerifyBinaryCommand(ILogger<VerifyPdfCommand> logger, IDigitalSignatureService digitalSignatureService, IEventService eventService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _digitalSignatureService = digitalSignatureService;
            _eventService = eventService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<VerifyBinaryResponseModel> Execute(VerifyBinaryModel model)
        {
            _logger.LogInformation($"Verifying file {model.FileName} for digital signatures.");

            var requestModel = CreateRequestModel(model);
            var response = await _digitalSignatureService.VerifyBinary(requestModel);

            await RecordEvent(model, response);
            
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
        
        private async Task RecordEvent(VerifyBinaryModel model, InternalVerificationResponseModel response)
        {
            var userId = _httpContextAccessor.GetUserIdFromClaims();
            
            var @event = new Event
            {
                Type = EventType.Verification,
                InputDocumentName = model.FileName,
                InputDocumentB64 = model.B64Bytes,
                OutputDocumentB64 = JsonSerializer.Serialize(response),
                TriggeredById = userId,
                IsSuccessful = response.ValidSignaturesCount == response.SignaturesCount,
                Error = string.Join(";",response.JaxbModel.SignatureOrTimestamp.SelectMany(x=>x.Errors))
            };

            await _eventService.RecordEvent(@event);
        }
    }
}