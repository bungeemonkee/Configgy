using Configgy.Utilities;
using System.Reflection;

namespace Configgy.Transfomers
{
    /// <summary>
    /// A value transformer that takes a base 64 encoded and RSA encrypted string and returns the decrypted string.
    /// </summary>
    public class DecryptionTransformerAttribute : ValueTransformerAttributeBase
    {
        public override string Transform(string value, string valueName, PropertyInfo property)
        {
            return value == null
                ? null
                : EncryptionUtility.Decrypt(value);
        }
    }
}
