using System.Threading.Tasks;

namespace Application.Signature.Commands.SignBinary
{
    public interface ISignBinaryCommand
    {
        Task<SignedBinaryResponseModel> Execute(SignBinaryModel model);
    }
}