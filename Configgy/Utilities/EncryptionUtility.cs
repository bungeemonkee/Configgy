using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Configgy.Utilities
{
    public static class EncryptionUtility
    {
        /// <summary>
        /// Decrypts a value encrypted using the <see cref="Encrypt(string,X509Certificate2)"/> function.
        /// There must be a certificate on this computer accessible by the current user that contains a
        /// public key matching the one used to do the encryption AND the associated private key.
        /// </summary>
        /// <param name="value">The value encrypted with the <see cref="Encrypt(string,X509Certificate2)"/> method.</param>
        /// <remarks>
        /// The format of the expected binary (after base 64 un-encoding) is the following:
        /// [AES key length in bytes] - Length: 4 bytes
        /// [AES block size in bytes] - Length: 4 bytes
        /// [AES key encrypted length in bytes] - Length: 4 bytes
        /// [AES initialization vector encrypted length in bytes] - Length: 4 bytes
        /// [Certificate public key length in bytes] - Length: 4 bytes
        /// [Certificate public key] - Length: Public key length
        /// [Encrypted AES key] - Length: AES key encrypted length bytes
        /// [Encrypted AES initialization vector] - Length: AES Block size bytes
        /// [Encrypted value] - Length: remaining data length
        /// </remarks>
        /// <returns>The decrypted value.</returns>
        public static string Decrypt(string value)
        {
            const int keyLengthPosition = 0;
            const int blockSizeLengthPosition = 4;
            const int keyEncryptedLengthPosition = 8;
            const int ivEncryptedLengthPosition = 12;
            const int publicKeyLengthPosition = 16;
            const int publicKeyPosition = 20;

            // Base 64 un-encode the value
            var bytes = Convert.FromBase64String(value);

            // Extract the AES key length, AES block size, encrypted AES key length, and encrypted AES initialization vector length
            var aesKeyLength = BitConverter.ToInt32(bytes, keyLengthPosition);
            var aesBlockSize = BitConverter.ToInt32(bytes, blockSizeLengthPosition);
            var aesKeyEncryptedLength = BitConverter.ToInt32(bytes, keyEncryptedLengthPosition);
            var aesInitializationVectorEncryptedLength = BitConverter.ToInt32(bytes, ivEncryptedLengthPosition);
            var publicKeyLength = BitConverter.ToInt32(bytes, publicKeyLengthPosition);

            // Extract the public key
            var publicKey = new byte[publicKeyLength];
            Array.Copy(bytes, publicKeyPosition, publicKey, 0, publicKeyLength);

            // Find the certificate for this public key
            var certificate = FindCertificateByPublicKey(publicKey);
            if (certificate == null)
            {
                throw new InvalidOperationException("No matching certificate found.");
            }

            // Find the private key for this certificate
            var rsa = certificate.PrivateKey as RSACryptoServiceProvider;
            if (rsa == null)
            {
                throw new InvalidOperationException("Certificate does not contain an RSA private key.");
            }

            // Extract the encrypted AES key
            var keyPosition = publicKeyPosition + publicKeyLength;
            var aesKey = new byte[aesKeyEncryptedLength];
            Array.Copy(bytes, keyPosition, aesKey, 0, aesKeyEncryptedLength);

            // Extract the encrypted AES initialization vector
            var ivPosition = keyPosition + aesKeyEncryptedLength;
            var aesInitializationVector = new byte[aesInitializationVectorEncryptedLength];
            Array.Copy(bytes, ivPosition, aesInitializationVector, 0, aesInitializationVectorEncryptedLength);

            // Decrypt the AES key and initialization vector using the RSA key
            aesKey = rsa.Decrypt(aesKey, false);
            aesInitializationVector = rsa.Decrypt(aesInitializationVector, false);

            // Chop off the bytes that contain the actual value
            var valuePosition = ivPosition + aesInitializationVectorEncryptedLength;
            var valueLength = bytes.Length - valuePosition;
            var valueBytes = new byte[valueLength];
            Array.Copy(bytes, valuePosition, valueBytes, 0, valueLength);

            // Decrypt the actual value using the aes encryption and return the resulting string
            using (var aes = new AesManaged
            {
                KeySize = 8 * aesKeyLength,
                BlockSize = 8 * aesBlockSize,
                Mode = CipherMode.CBC,
                Key = aesKey,
                IV = aesInitializationVector
            })
            using (var transform = aes.CreateDecryptor())
            using (var memoryOut = new MemoryStream())
            using (var memoryIn = new MemoryStream(valueBytes))
            using (var crypto = new CryptoStream(memoryIn, transform, CryptoStreamMode.Read))
            {
                crypto.CopyTo(memoryOut);
                return Encoding.UTF8.GetString(memoryOut.ToArray());
            }
        }

        /// <summary>
        /// Encrypts a value using the requested certificate.
        /// The certificate must have a valid RSA public key.
        /// </summary>
        /// <param name="value">The value to be encrypted.</param>
        /// <param name="certificate">The certificate used to encrypt the value.</param>
        /// <remarks> 
        /// First a cryptographically random AES key is generated.
        /// The value is encrypted using that AES key.
        /// The AES key is then encrypted using the RSA key from the certificate.
        /// Then the encrypted value is joined with the encrypted AES key.
        /// That result is base 64 encoded and returned.
        /// 
        /// The format of the result in binary (before base 64 encoding) is the following:
        /// [AES key length in bytes] - Length: 4 bytes
        /// [AES block size in bytes] - Length: 4 bytes
        /// [AES key encrypted length in bytes] - Length: 4 bytes
        /// [AES initialization vector encrypted length in bytes] - Length: 4 bytes
        /// [Certificate public key length in bytes] - Length: 4 bytes
        /// [Certificate public key] - Length: Public key length
        /// [Encrypted AES key] - Length: AES key encrypted length bytes
        /// [Encrypted AES initialization vector] - Length: AES Block size bytes
        /// [Encrypted value] - Length: remaining data length
        /// </remarks>
        /// <returns>The encrypted, base64-encoded value.</returns>
        public static string Encrypt(string value, X509Certificate2 certificate)
        {
            const int keySizeBits = 256;
            const int keySizeBytes = keySizeBits / 8;
            const int blockSizeBits = 128;
            const int blockSizeBytes = blockSizeBits / 8;

            // Get the RSA key from the certificate
            var rsa = certificate.PublicKey.Key as RSACryptoServiceProvider;
            if (rsa == null)
            {
                throw new InvalidOperationException("Certificate does not contain an RSA public key.");
            }

            // Get the public key as a byte array
            var publicKey = certificate.GetPublicKey();

            using (var aes = new AesManaged
            {
                KeySize = keySizeBits,
                BlockSize = blockSizeBits,
                Mode = CipherMode.CBC
            })
            {
                // Generate a cryptographically random initialization vector and key
                aes.GenerateIV();
                aes.GenerateKey();

                if (aes.IV.Length != blockSizeBytes)
                {
                    throw new InvalidOperationException("AES IV size is not equal to the block size.");
                }

                // Encrypt the AES key and initialization vector
                var keyBytes = rsa.Encrypt(aes.Key, false);
                var ivBytes = rsa.Encrypt(aes.IV, false);

                using (var memory = new MemoryStream())
                {
                    // Write the AES key length and block size
                    memory.WriteInt(keySizeBytes);
                    memory.WriteInt(blockSizeBytes);

                    // Write the sizes of the encrypted AES key and initialization vector
                    memory.WriteInt(keyBytes.Length);
                    memory.WriteInt(ivBytes.Length);

                    // Write the size of the certificate public key
                    memory.WriteInt(publicKey.Length);

                    // Write the public key
                    memory.Write(publicKey, 0, publicKey.Length);

                    // Write the encrypted AES key and initialization vector
                    memory.Write(keyBytes, 0, keyBytes.Length);
                    memory.Write(ivBytes, 0, ivBytes.Length);

                    // Encrypt and write the actual value using the aes encryption
                    using (var transform = aes.CreateEncryptor())
                    using (var crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write))
                    {
                        var bytes = Encoding.UTF8.GetBytes(value);
                        crypto.Write(bytes, 0, bytes.Length);
                        crypto.FlushFinalBlock();
                    }

                    // Return the base 64 encoded result
                    return Convert.ToBase64String(memory.ToArray(), Base64FormattingOptions.InsertLineBreaks);
                }
            }
        }

        /// <summary>
        /// Find a certificate by its public key.
        /// </summary>
        /// <param name="publicKey">The public key of the certificate to find.</param>
        /// <returns>The certificate or null.</returns>
        public static X509Certificate2 FindCertificateByPublicKey(byte[] publicKey)
        {
            var certificates = FindCertificates(x => BytesAreEqual(x.GetPublicKey(), publicKey));

            return certificates
                .OrderByDescending(x => x.HasPrivateKey)
                .ThenByDescending(x => x.NotAfter)
                .FirstOrDefault();
        }

        /// <summary>
        /// Find certificates matching some predicate.
        /// </summary>
        /// <param name="predicate">The function used to determine which certificates to include.</param>
        /// <returns>The matching certificates.</returns>
        public static IEnumerable<X509Certificate2> FindCertificates(Func<X509Certificate2, bool> predicate)
        {
            var stores = Enum.GetValues(typeof(StoreName)).Cast<StoreName>().ToArray();
            var locations = Enum.GetValues(typeof(StoreLocation)).Cast<StoreLocation>().ToArray();

            var certificates = new List<X509Certificate2>();

            foreach (var store in stores)
            {
                foreach (var location in locations)
                {
                    X509Store source = null;
                    try
                    {
                        source = new X509Store(store, location);
                        source.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                        certificates.AddRange(source.Certificates
                            .Cast<X509Certificate2>()
                            .Where(x => !x.Archived)
                            .Where(x => x.NotAfter >= DateTime.Now)
                            .Where(x => x.NotBefore <= DateTime.Now)
                            .Where(predicate)
                            .Where(x => x.Verify())
                            );
                    }
                    catch
                    {
                        // Just try the next one
                    }
                    finally
                    {
                        source?.Close();
                    }
                }
            }

            return certificates;
        }

        private static bool BytesAreEqual(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            if (a == null || b == null) return false;

            if (a.Count != b.Count) return false;

            for (var i = 0; i < a.Count; ++i)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }
    }
}
