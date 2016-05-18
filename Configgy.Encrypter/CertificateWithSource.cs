using System.Security.Cryptography.X509Certificates;

namespace Configgy.Encrypter
{
    public class CertificateWithSource
    {
        public StoreName StoreName { get; set; }

        public StoreLocation StoreLocation { get; set; }

        public X509Certificate2 Certifcate { get; set; }

        public string DisplayName => $"Store: {StoreName} Location: {StoreLocation} Thumbprint: {Certifcate.Thumbprint} Name: {Certifcate.FriendlyName}";
    }
}
