using System;
using System.IO;
using System.Threading.Tasks;
using Application.Signature.Commands.SignPdf;
using Microsoft.Extensions.Logging;

namespace Application.Signature.Commands.DownloadSignedPdf
{
    public class DownloadSignedPdfCommand : IDownloadSignedPdfCommand
    {
        private readonly ILogger<DownloadSignedPdfCommand> _logger;
        private readonly ISignPdfCommand _signPdfCommand;

        public DownloadSignedPdfCommand(ILogger<DownloadSignedPdfCommand> logger, ISignPdfCommand signPdfCommand)
        {
            _logger = logger;
            _signPdfCommand = signPdfCommand;
        }

        public async Task<Stream> Execute(SignPdfModel model)
        {
            _logger.LogInformation($"Downloading signed pdf {model.FileName}");
            var signedPdf = await _signPdfCommand.Execute(model);

            var pdfBytes = Convert.FromBase64String(signedPdf.SignedB64Bytes);

            return new MemoryStream(pdfBytes);
        }
    }
}