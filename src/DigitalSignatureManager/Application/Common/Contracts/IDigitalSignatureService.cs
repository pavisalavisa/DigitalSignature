using System.Threading.Tasks;

namespace Application.Common.Contracts
{
    public interface IDigitalSignatureService
    {
        Task SignPdf();
        Task SignBinary();
        Task VerifyPdf();
        Task VerifyBinary();
        Task<bool> IsHealthy();
    }
}