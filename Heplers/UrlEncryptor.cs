using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementSystem.Helpers
{
    public static class UrlEncryptor
    {
        // 16-character encryption key for AES
        private static readonly string EncryptionKey = "pjsGLNYrMqU6wny4";

        // Encrypt method
        public static string Encrypt(string text)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = new byte[16]; // 16-byte IV

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                    // IMPORTANT: StreamWriter and CryptoStream are disposed here, flushing all data to MemoryStream
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }


        // Decrypt method
        public static string Decrypt(string encryptedText)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = new byte[16];

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}
