using Common.Models;

namespace Application.Common.ErrorManagement.ErrorCodes
{
    public class SignatureErrorCodes
    {
        public static readonly CodedError Unknown = new CodedError("S001", "Unknown signature error");
        public static readonly CodedError InvalidPdfSignatureModel = new CodedError("S002", "Invalid PDF signature model");
    }
}