using System.Threading.Tasks;
using Application.Common.Models.DigitalSignatureModels;

namespace Application.Common.Contracts
{
    public interface IDigitalSignatureService
    {
        Task<InternalSignatureResponseModel> SignPdf(InternalSignatureRequestModel requestModel);
        Task<InternalSignatureResponseModel> SignBinary(InternalSignatureRequestModel requestModel);
        Task VerifyPdf();
        Task VerifyBinary();
        Task<bool> IsHealthy();
    }
}