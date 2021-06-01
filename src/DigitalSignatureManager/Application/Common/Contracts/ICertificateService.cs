using System.Threading.Tasks;
using Application.Common.Models.DigitalSignatureModels;

namespace Application.Common.Contracts
{
    public interface ICertificateService
    {
        Task<InternalCertificateSignatureModel> GetUserCertificate(int userId);
    }
}