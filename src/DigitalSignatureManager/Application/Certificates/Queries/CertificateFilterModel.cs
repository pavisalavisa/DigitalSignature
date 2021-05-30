using System;
using Application.Common.ErrorManagement.ErrorCodes;
using Application.Common.Models;
using Common.Extensions;
using FluentValidation;

namespace Application.Certificates.Queries
{
    public class CertificateFilterModel : FilterPagingQueryModel
    {
        public string OwnerName { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? UpdatedFrom { get; set; }
        public DateTime? UpdatedTo { get; set; }
        public bool? IsRevoked { get; set; }

        public class CertificateFilterModelValidator : AbstractValidator<CertificateFilterModel>
        {
            public CertificateFilterModelValidator()
            {
                RuleFor(x => x.CreatedFrom)
                    .Must((model, createdFrom) => createdFrom <= model.CreatedTo)
                    .When(model => model.CreatedFrom.HasValue && model.CreatedTo.HasValue)
                    .WithCodedErrorMessage(CertificateErrorCodes.InvalidCertificateFilter with {Details = "Created from value should be before created to."});

                RuleFor(x => x.UpdatedFrom)
                    .Must((model, updatedFrom) => updatedFrom <= model.UpdatedTo)
                    .When(model => model.UpdatedFrom.HasValue && model.UpdatedTo.HasValue)
                    .WithCodedErrorMessage(CertificateErrorCodes.InvalidCertificateFilter with {Details = "Updated from value should be before updated to."});
            }
        }
    }
}