using System;
using System.Collections.Generic;

namespace Application.Verification.Commands.VerifyPdf
{
    public class VerifyPdfResponseModel
    {
        public int SignaturesCount { get; set; }
        public int ValidSignaturesCount { get; set; }
        public string DocumentName { get; set; }
        public List<VerifyPdfSignatureModel> Signatures { get; set; }

        public class VerifyPdfSignatureModel
        {
            public string Result { get; set; }
            public List<string> Errors { get; set; }
            public List<string> Warnings { get; set; }
            public string Id { get; set; }
            public DateTime SignatureTime { get; set; }
            public string SignedBy { get; set; }
            public string SignatureFormat { get; set; }
        }
    }
}