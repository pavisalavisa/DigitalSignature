using System;
using System.Collections.Generic;

namespace Application.Verification.Commands.VerifyBinary
{
    public class VerifyBinaryResponseModel
    {
        public int SignaturesCount { get; set; }
        public int ValidSignaturesCount { get; set; }
        public string DocumentName { get; set; }
        public List<VerifyBinarySignatureModel> Signatures { get; set; }

        public class VerifyBinarySignatureModel
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