using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Configgy.Utilities
{
    public static class EncryptionUtility
    {

        /// <summary>
        /// Decrypts a value using the requested certificate.
        /// The certificate must have a valid RSA private key.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="certificateThumbprint"></param>
        /// <param name="storeName"></param>
        /// <param name="certificateStore"></param>
        /// <remarks>
        /// The format of the expected binary (after base 64 unencoding) is the following:
        /// [AES key length in bytes] - Length: 4 bytes
        /// [AES block size in bytes] - Length: 4 bytes
        /// [AES key encrypted length in bytes] - Length: 4 bytes
        /// [AES initialization vector encrypted length in bytes] - Length: 4 bytes
        /// [Encrypted AES key] - Length: AES key encrypted length bytes
        /// [Encrypted AES initialization vector] - Length: AES Block size bytes
        /// [Encrypted value] - Length: remaining data length
        /// </remarks>
        /// <returns></returns>
        public static string Decrypt(string value, string certificateThumbprint, StoreName storeName, StoreLocation certificateStore)
        {
            const int keyLengthPosition = 0;
            const int blockSizeLengthPosition = 4;
            const int keyEncryptedLengthPosition = 8;
            const int ivEncryptedLengthPosition = 12;
            const int keyPosition = 16;

            // Get the certificate and its RSA key
            var certificate = GetCertificate(certificateThumbprint, storeName, certificateStore);
            var rsa = certificate?.PrivateKey as RSACryptoServiceProvider;
            if (rsa == null)
            {
                throw new InvalidOperationException("Certificate does not contain an RSA public key.");
            }

            // Base 64 unencode the value
            var bytes = Convert.FromBase64String(value);

            // Extract the AES key length, AES block size, encrypted AES key length, and encrypted AES initialization vector length
            var aesKeyLength = BitConverter.ToInt32(bytes, keyLengthPosition);
            var aesBlockSize = BitConverter.ToInt32(bytes, blockSizeLengthPosition);
            var aesKeyEncryptedLength = BitConverter.ToInt32(bytes, keyEncryptedLengthPosition);
            var aesInitializationVectorEncryptedLength = BitConverter.ToInt32(bytes, ivEncryptedLengthPosition);

            // Extract the encrypted AES key
            var aesKey = new byte[aesKeyEncryptedLength];
            Array.Copy(bytes, keyPosition, aesKey, 0, aesKeyEncryptedLength);

            // Extract the encrypted AES initialization vector
            var aesInitializationVector = new byte[aesInitializationVectorEncryptedLength];
            var ivPosition = keyPosition + aesKeyEncryptedLength;
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
        /// <param name="value"></param>
        /// <param name="certificateThumbprint"></param>
        /// <param name="storeName"></param>
        /// <param name="certificateStore"></param>
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
        /// [Encrypted AES key] - Length: AES key encrypted length bytes
        /// [Encrypted AES initialization vector] - Length: AES Block size bytes
        /// [Encrypted value] - Length: remaining data length
        /// </remarks>
        /// <returns></returns>
        public static string Encrypt(string value, string certificateThumbprint, StoreName storeName, StoreLocation certificateStore)
        {
            const int KeySizeBits = 256;
            const int KeySizeBytes = KeySizeBits / 8;
            const int BlockSizeBits = 128;
            const int BlockSizeBytes = BlockSizeBits / 8;

            // Get the certificate and its RSA key
            var certificate = GetCertificate(certificateThumbprint, storeName, certificateStore);
            var rsa = certificate?.PublicKey?.Key as RSACryptoServiceProvider;
            if (rsa == null)
            {
                throw new InvalidOperationException("Certificate does not contain an RSA public key.");
            }

            using (var aes = new AesManaged
            {
                KeySize = KeySizeBits,
                BlockSize = BlockSizeBits,
                Mode = CipherMode.CBC
            })
            {
                aes.GenerateIV();
                aes.GenerateKey();

                if (aes.IV.Length != BlockSizeBytes)
                {
                    throw new InvalidOperationException("AES IV size is not equal to the block size.");
                }

                using (var memory = new MemoryStream())
                {
                    // Write the AES key length and block size
                    memory.WriteInt(KeySizeBytes);
                    memory.WriteInt(BlockSizeBytes);

                    // Encrypt the AES key and initialization vector
                    var keyBytes = rsa.Encrypt(aes.Key, false);
                    var ivBytes = rsa.Encrypt(aes.IV, false);

                    // Write the sizes of the encrypted AES key and initialization vector
                    memory.WriteInt(keyBytes.Length);
                    memory.WriteInt(ivBytes.Length);

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
    }
}
