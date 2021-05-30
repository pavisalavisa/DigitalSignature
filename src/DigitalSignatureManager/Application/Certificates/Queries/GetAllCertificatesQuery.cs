using System;
using System.Linq.Expressions;
using Application.Common.Base.Queries;
using Application.Common.Contracts;
using Common;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Certificates.Queries
{
    public class GetAllCertificatesQuery : BaseFilterQuery<Certificate,CertificateModel, CertificateFilterModel>, IGetAllCertificatesQuery
    {
        public GetAllCertificatesQuery(ILogger<BaseFilterQuery<Certificate, CertificateModel, CertificateFilterModel>> logger, IDigitalSignatureManagerDbContext context) : base(logger, context)
        {
        }

        protected override Expression<Func<Certificate, bool>> GetFilterExpression(CertificateFilterModel filterModel)
        {
            Expression<Func<Certificate, bool>> predicate = x => true;

            if (filterModel.IsRevoked != null)
                predicate = predicate.And(x => x.IsRevoked == filterModel.IsRevoked);

            if (filterModel.OwnerName != null)
                predicate = predicate.And(x => x.Owner.UserName.Contains(filterModel.OwnerName));

            if (filterModel.CreatedFrom != null)
                predicate = predicate.And(x => x.Created>=filterModel.CreatedFrom);

            if (filterModel.CreatedTo  != null)
                predicate = predicate.And(x => x.Created <=filterModel.CreatedTo);

            if (filterModel.UpdatedFrom != null)
                predicate = predicate.And(x => x.Updated>=filterModel.UpdatedFrom);

            if (filterModel.UpdatedTo != null)
                predicate = predicate.And(x => x.Updated <= filterModel.UpdatedTo);

            return predicate;
        }

        protected override Expression<Func<Certificate, CertificateModel>> GetMappingExpression() =>
            x => new CertificateModel
            {
                Id = x.Id,
                Created = x.Created,
                Updated = x.Updated,
                IsRevoked = x.IsRevoked,
                OwnerName = x.Owner.UserName
            };
    }
}