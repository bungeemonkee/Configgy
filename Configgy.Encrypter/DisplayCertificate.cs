using System;
using System.Security.Cryptography.X509Certificates;

namespace Configgy.Encrypter
{
    public class DisplayCertificate
    {
        public X509Certificate2 Certificate { get; }

        public string DisplayName => $"Thumbprint: {Certificate.Thumbprint} Private Key: {(Certificate.HasPrivateKey ? "Yes" : "No ")} Expiration: {Certificate.NotAfter:yyyy-MM-dd} Name: {Certificate.FriendlyName}";

        public DisplayCertificate(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            Certificate = certificate;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public override bool Equals(object obj)
        {
            var other = obj as DisplayCertificate;
            if (other == null) return false;

            return other.Certificate.Thumbprint == Certificate.Thumbprint;
        }

        public override int GetHashCode()
        {
            return Certificate.Thumbprint.GetHashCode();
        }
    }
}
