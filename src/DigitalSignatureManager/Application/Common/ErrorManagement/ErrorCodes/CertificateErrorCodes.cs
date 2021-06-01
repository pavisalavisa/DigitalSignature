using Common.Models;

namespace Application.Common.ErrorManagement.ErrorCodes
{
    public static class CertificateErrorCodes
    {
        public static readonly CodedError Unknown = new("C001", "Unknown certificate error");
        public static readonly CodedError InvalidCertificateFilter = new("C002", "Invalid certificate filter");
        public static readonly CodedError InvalidCertificateModel = new("C003", "Invalid certificate model");
        public static readonly CodedError InvalidCertificateBytes = new("C004", "Invalid certificate bytes provided");
        public static readonly CodedError UserCertificateDoesntExist = new("C005", "User certificate does not exist");
    }
}