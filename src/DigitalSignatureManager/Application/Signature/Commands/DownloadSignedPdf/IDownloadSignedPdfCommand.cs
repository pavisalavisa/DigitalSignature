using System.IO;
using System.Threading.Tasks;
using Application.Signature.Commands.SignPdf;

namespace Application.Signature.Commands.DownloadSignedPdf
{
    public interface IDownloadSignedPdfCommand
    {
        Task<Stream> Execute(SignPdfModel model);
    }
}