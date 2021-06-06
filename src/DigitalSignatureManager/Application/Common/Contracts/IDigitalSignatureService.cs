using System.Threading.Tasks;
using Application.Common.Models.DigitalSignatureModels;

namespace Application.Common.Contracts
{
    public interface IDigitalSignatureService
    {
        Task<InternalSignatureResponseModel> SignPdf(InternalSignatureRequestModel requestModel);
        Task<InternalSignatureResponseModel> SignBinary(InternalSignatureRequestModel requestModel);
        Task<InternalVerificationResponseModel> VerifyPdf(BaseFileRequestModel requestModel);
        Task<InternalVerificationResponseModel> VerifyBinary(InternalDetachedSignatureRequestModel requestModel);
        Task<bool> IsHealthy();
    }
}