using Microsoft.AspNetCore.Identity;

namespace Application.Signature.Commands.SignPdf
{
    public class SignedPdfResponseModel
    {
        public string FileName { get; set; }
        public string SignedB64Bytes { get; set; }
    }
}