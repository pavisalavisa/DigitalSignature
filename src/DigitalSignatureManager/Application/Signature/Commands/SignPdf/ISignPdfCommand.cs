using System.Threading.Tasks;

namespace Application.Signature.Commands.SignPdf
{
    public interface ISignPdfCommand
    {
        Task<SignedPdfResponseModel> Execute(SignPdfModel model);
    }
}