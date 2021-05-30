using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helpers
{
    public static class CryptographyHelper
    {
        private const int TagBytes = 16;
        private const int NonceBytes = 12;

        public static string Encrypt(string plainText, string key) =>
            Convert.ToBase64String(EncryptBytesAesGcm(Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(key)));

        public static string Decrypt(string b64CipherText, string key) =>
            Encoding.UTF8.GetString(DecryptBytesAesGcm(Convert.FromBase64String(b64CipherText), Encoding.UTF8.GetBytes(key)));

        private static byte[] EncryptBytesAesGcm(byte[] toEncrypt, byte[] key, byte[] associatedData = null)
        {
            using var rng = new RNGCryptoServiceProvider();

            var tag = new byte[TagBytes];
            var nonce = new byte[NonceBytes];
            var cipherText = new byte[toEncrypt.Length];

            rng.GetBytes(tag);
            rng.GetBytes(nonce);

            using var cipher = new AesGcm(key);
            cipher.Encrypt(nonce, toEncrypt, cipherText, tag, associatedData);

            return Concat(tag, Concat(nonce, cipherText));
        }

        private static byte[] DecryptBytesAesGcm(byte[] cipherText, byte[] key, byte[] associatedData = null)
        {
            var tag = SubArray(cipherText, 0, TagBytes);
            var nonce = SubArray(cipherText, TagBytes, NonceBytes);

            var toDecrypt = SubArray(cipherText, TagBytes + NonceBytes, cipherText.Length - tag.Length - nonce.Length);
            var decryptedData = new byte[toDecrypt.Length];

            using var cipher = new AesGcm(key);
            cipher.Decrypt(nonce, toDecrypt, tag, decryptedData, associatedData);

            return decryptedData;
        }

        private static byte[] Concat(byte[] a, byte[] b)
        {
            var output = new byte[a.Length + b.Length];

            for (var i = 0; i < a.Length; i++)
            {
                output[i] = a[i];
            }

            for (var j = 0; j < b.Length; j++)
            {
                output[a.Length + j] = b[j];
            }

            return output;
        }

        private static byte[] SubArray(byte[] data, int start, int length)
        {
            var result = new byte[length];

            Array.Copy(data, start, result, 0, length);

            return result;
        }
    }
}