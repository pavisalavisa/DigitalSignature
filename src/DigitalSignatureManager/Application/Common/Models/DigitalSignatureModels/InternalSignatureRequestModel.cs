using Domain.Enums;

namespace Application.Common.Models.DigitalSignatureModels
{
    public class InternalSignatureRequestModel : BaseFileRequestModel
    {
        public InternalCertificateSignatureModel Certificate { get; set; }
        public SignatureProfile Profile { get; set; }
    }
}