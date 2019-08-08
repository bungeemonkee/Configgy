using Configgy.Utilities;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that takes a base 64 encoded and RSA encrypted string and returns the decrypted string.
    /// </summary>
    public class DecryptionTransformerAttribute : ValueTransformerAttributeBase
    {
        /// <inheritdoc cref="IValueTransformer.Transform"/>
        public override string Transform(IConfigProperty property, string value)
        {
            return value == null
                ? null
                : EncryptionUtility.Decrypt(value);
        }
    }
}
