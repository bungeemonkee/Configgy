using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Configgy.Transfomers
{
    public static class EncryptionUtility
    {
        public static X509Certificate2 GetCertificate(string certificateThumbprint, StoreName storeName, StoreLocation certificateStore)
        {
            var certificates = new X509Store(storeName, certificateStore);
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

        public static string Decrypt(string value, string certificateThumbprint, StoreName storeName, StoreLocation certificateStore)
        {
            var certificate = GetCertificate(certificateThumbprint, storeName, certificateStore);
            var rsa = certificate?.PrivateKey as RSACryptoServiceProvider;
            if (rsa == null)
            {
                throw new InvalidOperationException("Certificate does not contain an RSA public key.");
            }
            var bytes = Convert.FromBase64String(value);
            bytes = rsa.Decrypt(bytes, false);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string Encrypt(string value, string certificateThumbprint, StoreName storeName, StoreLocation certificateStore)
        {
            var certificate = GetCertificate(certificateThumbprint, storeName, certificateStore);
            var rsa = certificate?.PublicKey?.Key as RSACryptoServiceProvider;
            if (rsa == null)
            {
                throw new InvalidOperationException("Certificate does not contain an RSA public key.");
            }
            var bytes = Encoding.UTF8.GetBytes(value);
            bytes = rsa.Encrypt(bytes, false);
            return Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
        }
    }
}
