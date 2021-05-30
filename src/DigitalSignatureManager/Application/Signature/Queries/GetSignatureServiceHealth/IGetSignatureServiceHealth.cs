using System.Threading.Tasks;

namespace Application.Signature.Queries.GetSignatureServiceHealth
{
    public interface IGetSignatureServiceHealth
    {
        Task<bool> IsHealthy();
    }
}