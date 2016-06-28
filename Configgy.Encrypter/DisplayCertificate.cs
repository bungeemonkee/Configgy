using System.Security.Cryptography.X509Certificates;

namespace Configgy.Encrypter
{
    public class DisplayCertificate
    {
        public X509Certificate2 Certificate { get; set; }

        public string DisplayName => $"Thumbprint: {Certificate.Thumbprint} Private Key: {Certificate.HasPrivateKey} Name: {Certificate.FriendlyName}";
    }
}
