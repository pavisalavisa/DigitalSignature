namespace Application.Signature.Commands.SignBinary
{
    public class SignedBinaryResponseModel
    {
        public string FileName { get; set; }
        public string SignedB64Bytes { get; set; }
    }
}