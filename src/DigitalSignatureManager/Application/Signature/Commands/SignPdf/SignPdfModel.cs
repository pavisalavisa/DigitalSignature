using System.Security.Cryptography.X509Certificates;
using Application.Common.ErrorManagement.ErrorCodes;
using Common.Extensions;
using FluentValidation;

namespace Application.Signature.Commands.SignPdf
{
    public class SignPdfModel
    {
        public string FileName { get; set; }
        public string B64Bytes { get; set; }

        public class Validator : AbstractValidator<SignPdfModel>
        {
            public Validator()
            {
                RuleFor(x => x.FileName)
                    .NotEmpty();

                RuleFor(x => x.B64Bytes)
                    .IsB64String()
                    .WithCodedErrorMessage(SignatureErrorCodes.InvalidPdfSignatureModel with {Details = "B64Bytes must be a valid B64 string."});
            }
        }
    }
}