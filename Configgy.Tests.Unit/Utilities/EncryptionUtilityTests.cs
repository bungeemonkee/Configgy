using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Utilities;

namespace Configgy.Tests.Unit.Utilities
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
                .FindCertificates(x => x.HasPrivateKey)
                .FirstOrDefault();

            if (certificate == null)
            {
                Assert.Inconclusive("Unable to find a certificate suitable to preform the test.");
            }

            var encrypted = EncryptionUtility.Encrypt(value, certificate);

            var decrypted = EncryptionUtility.Decrypt(encrypted);

            Assert.AreEqual(value, decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Encrypt_With_No_Private_Key_Then_Decrypt_Throws_InvalidOperationException()
        {
            var value = "This is some text that should be encrypted then decrypted and still come out the same.";

            var certificate = EncryptionUtility
                .FindCertificates(x => !x.HasPrivateKey)
                // Make sure there is no other certificate with this public key and a private key as well
                .FirstOrDefault(x => !EncryptionUtility.FindCertificateByPublicKey(x.GetPublicKey()).HasPrivateKey);

            if (certificate == null)
            {
                Assert.Inconclusive("Unable to find a certificate suitable to preform the test.");
            }

            var encrypted = EncryptionUtility.Encrypt(value, certificate);

            var decrypted = EncryptionUtility.Decrypt(encrypted);

            Assert.AreEqual(value, decrypted);
        }
    }
}
