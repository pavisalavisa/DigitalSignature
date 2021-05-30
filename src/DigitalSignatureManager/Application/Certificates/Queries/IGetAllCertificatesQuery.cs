using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Certificates.Queries
{
    public interface IGetAllCertificatesQuery
    {
        Task<PagingResultModel<CertificateModel>> Query(CertificateFilterModel model);
    }
}