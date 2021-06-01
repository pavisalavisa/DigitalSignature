namespace Application.Common.Models.DigitalSignatureModels
{
    public class InternalSignatureRequestModel
    {
        public string FileName { get; set; }
        public string B64Bytes { get; set; }
        public InternalCertificateSignatureModel Certificate { get; set; }
    }
}