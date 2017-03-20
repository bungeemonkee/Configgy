using Configgy.Utilities;
using System.Reflection;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that takes a base 64 encoded and RSA encrypted string and returns the decrypted string.
    /// </summary>
    public class DecryptionTransformerAttribute : ValueTransformerAttributeBase
    {
        public override string Transform(string value, string valueName, ICustomAttributeProvider property)
        {
            return value == null
                ? null
                : EncryptionUtility.Decrypt(value);
        }
    }
}
