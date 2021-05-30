using Application.Common.ErrorManagement.ErrorCodes;
using Common.Extensions;
using FluentValidation;

namespace Application.Users.Commands.AssignCertificate
{
    public class CertificateAssignmentModel
    {
        public string B64Certificate { get; set; }
        public string CertificatePassword { get; set; }

        public class CertificateAssignmentModelValidator : AbstractValidator<CertificateAssignmentModel>
        {
            public CertificateAssignmentModelValidator()
            {
                RuleFor(x => x.B64Certificate)
                    .NotEmpty()
                    .IsB64String()
                    .WithCodedErrorMessage(CertificateErrorCodes.InvalidCertificateModel with{ Details = "Certificate must be a valid B64 string."});

                RuleFor(x=>x.CertificatePassword)
                    .NotEmpty()
                    .WithCodedErrorMessage(CertificateErrorCodes.InvalidCertificateModel with{ Details = "Certificate password must be provided."});
            }
        }
    }
}