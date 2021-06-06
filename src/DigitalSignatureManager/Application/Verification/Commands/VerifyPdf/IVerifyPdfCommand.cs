using System.Threading.Tasks;

namespace Application.Verification.Commands.VerifyPdf
{
    public interface IVerifyPdfCommand
    {
        Task<VerifyPdfResponseModel> Execute(VerifyPdfModel model);
    }
}