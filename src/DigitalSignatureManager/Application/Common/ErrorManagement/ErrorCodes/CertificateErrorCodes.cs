using Common.Models;

namespace Application.Common.ErrorManagement.ErrorCodes
{
    public static class CertificateErrorCodes
    {
        public static readonly CodedError Unknown = new CodedError("C001", "Unknown certificate error");
        public static readonly CodedError InvalidCertificateFilter = new CodedError("C002", "Invalid certificate filter");
        public static readonly CodedError InvalidCertificateModel = new CodedError("C003", "Invalid certificate model");
    }
}