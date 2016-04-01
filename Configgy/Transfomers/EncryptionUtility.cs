using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Configgy.Transfomers
{
    public static class EncryptionUtility
    {
        public static X509Certificate2 GetCertificate(string certificateThumbprint, StoreLocation certificateStore)
        {
            var certificates = new X509Store(certificateStore);
            try
            {
                certificates.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                foreach (var certificate in certificates.Certificates)
                {
                    if (certificate.Thumbprint == certificateThumbprint) return certificate;
                }
            }
            finally
            {
                certificates.Close();
            }

            // If we got this far no matching certificate was found
            throw new InvalidOperationException($"No matching certificate was found. Store: {certificateStore} Thumbprint: {certificateThumbprint}");
        }

        public static string Decrypt(string value, string certificateThumbprint, StoreLocation certificateStore)
        {
            var certificate = GetCertificate(certificateThumbprint, certificateStore);
            var encrypt = (RSACryptoServiceProvider)certificate.PrivateKey;
            var bytes = Convert.FromBase64String(value);
            bytes = encrypt.Encrypt(bytes, false);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string Encrypt(string value, string certificateThumbprint, StoreLocation certificateStore)
        {
            var certificate = GetCertificate(certificateThumbprint, certificateStore);
            var encrypt = (RSACryptoServiceProvider)certificate.PrivateKey;
            var bytes = Encoding.UTF8.GetBytes(value);
            bytes = encrypt.Encrypt(bytes, false);
            return Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
        }
    }
}
