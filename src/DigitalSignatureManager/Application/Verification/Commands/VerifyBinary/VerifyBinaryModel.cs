namespace Application.Verification.Commands.VerifyBinary
{
    public class VerifyBinaryModel
    {
        public string FileName { get; set; }
        public string B64Bytes { get; set; }
        public string B64XadesSignature { get; set; }
    }
}