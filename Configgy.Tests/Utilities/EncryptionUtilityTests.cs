using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Utilities;

namespace Configgy.Tests.Utilities
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EncryptionUtilityTests
    {
        [TestMethod]
        public void Encrypt_Then_Decrypt_Result_Matches_Input()
        {
            var value = "This is some text that should be encrypted then decrypted and still come out the same.";

            var certificate = EncryptionUtility
                .FindCertificates(x => HasPrivateKey(x) && HasPublicKey(x))
                .FirstOrDefault();

            if (certificate == null)
                Assert.Inconclusive("Unable to find a certificate suitable to preform the test.");

            var encrypted = EncryptionUtility.Encrypt(value, certificate);

            var decrypted = EncryptionUtility.Decrypt(encrypted);

            Assert.AreEqual(value, decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Encrypt_With_No_Private_Key_Then_Decrypt_Throws_InvalidOperationException()
        {
            var value = "This is some text that should be encrypted then decrypted and still come out the same.";

            var certificate = EncryptionUtility
                .FindCertificates(x => !HasPrivateKey(x) && HasPublicKey(x))
                // Make sure there is no other certificate with this public key and a private key as well
                .FirstOrDefault(x => ! HasPrivateKey(EncryptionUtility.FindCertificateByPublicKey(x.GetPublicKey())));

            if (certificate == null)
                Assert.Inconclusive("Unable to find a certificate suitable to preform the test.");

            var encrypted = EncryptionUtility.Encrypt(value, certificate);

            var decrypted = EncryptionUtility.Decrypt(encrypted);

            Assert.AreEqual(value, decrypted);
        }

        private static bool HasPrivateKey(X509Certificate2 certificate)
        {
            var testData = new byte[]
            {
                1,2,3,4,5,6,7,8,9,0
            };

            try
            {
                var key = certificate.GetRSAPrivateKey();
                if (key == null)
                    return false;

                var encrypted = key.Encrypt(testData, RSAEncryptionPadding.OaepSHA256);
                var decrypted = key.Decrypt(encrypted, RSAEncryptionPadding.OaepSHA256);

                CollectionAssert.AreEqual(testData, decrypted);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool HasPublicKey(X509Certificate2 certificate)
        {
            try
            {
                var key = certificate.GetPublicKey();
                return key != null;
            }
            catch
            {
                return false;
            }
        }
    }
}