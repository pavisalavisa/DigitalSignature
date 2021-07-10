using Application.Common.ErrorManagement.ErrorCodes;
using Common.Extensions;
using Domain.Enums;
using FluentValidation;

namespace Application.Signature.Commands.SignBinary
{
    public class SignBinaryModel
    {
        public string FileName { get; set; }
        public string B64Bytes { get; set; }
        public SignatureProfile Profile { get; set; }
        
        public class Validator : AbstractValidator<SignBinaryModel>
        {
            public Validator()
            {
                RuleFor(x => x.FileName)
                    .NotEmpty();

                RuleFor(x => x.B64Bytes)
                    .IsB64String()
                    .WithCodedErrorMessage(SignatureErrorCodes.InvalidBinarySignatureModel with {Details = "B64Bytes must be a valid B64 string."});
                
                RuleFor(x => x.Profile)
                    .NotNull()
                    .IsInEnum()
                    .WithCodedErrorMessage(SignatureErrorCodes.InvalidPdfSignatureModel with {Details = "Profile must be one of the following values: B, T, LT, LTA."});
            }
        }
    }
}