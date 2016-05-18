using Configgy.Utilities;
using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Configgy.Transfomers
{
    /// <summary>
    /// A value transformer that takes a base 64 encoded and RSA encrypted string and returns the decrypted string.
    /// </summary>
    public class DecryptionTransformerAttribute : ValueTransformerAttributeBase
    {
        public string CertificateThumbprint { get; private set; }

        public StoreLocation CertificateStoreLocation { get; set; }

        public StoreName CertificateStoreName { get; set; }

        public DecryptionTransformerAttribute(string certificateThumbprint)
        {
            if (certificateThumbprint == null)
            {
                throw new ArgumentNullException(nameof(certificateThumbprint));
            }

            CertificateThumbprint = certificateThumbprint;
            CertificateStoreLocation = StoreLocation.LocalMachine;
            CertificateStoreName = StoreName.My;
        }

        public override string TransformValue(string value, string valueName, PropertyInfo property)
        {
            return EncryptionUtility.Decrypt(value, CertificateThumbprint, CertificateStoreName, CertificateStoreLocation);
        }
    }
}
