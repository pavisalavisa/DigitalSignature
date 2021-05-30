using System;
using System.Security.Cryptography.X509Certificates;
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

                RuleFor(x => x)
                    .Must(ValidX509Certificate)
                    .WithCodedErrorMessage(CertificateErrorCodes.InvalidCertificateBytes with {Details = "The provided b64 string is not a valid X509 certificate."});
            }

            private bool ValidX509Certificate(CertificateAssignmentModel model)
            {
                try
                {
                    // ReSharper disable once ObjectCreationAsStatement
                    new X509Certificate(Convert.FromBase64String(model.B64Certificate), model.CertificatePassword);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}