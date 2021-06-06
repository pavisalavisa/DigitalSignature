namespace Application.Common.Models.DigitalSignatureModels
{
    public class InternalDetachedSignatureRequestModel : BaseFileRequestModel
    {
        public string B64XadesSignature { get; set; }
    }
}