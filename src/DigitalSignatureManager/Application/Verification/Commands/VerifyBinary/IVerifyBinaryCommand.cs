using System.Threading.Tasks;

namespace Application.Verification.Commands.VerifyBinary
{
    public interface IVerifyBinaryCommand
    {
        Task<VerifyBinaryResponseModel> Execute(VerifyBinaryModel model);
    }
}