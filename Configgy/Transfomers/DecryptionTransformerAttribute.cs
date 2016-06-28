using Configgy.Utilities;
using System.Reflection;

namespace Configgy.Transfomers
{
    /// <summary>
    /// A value transformer that takes a base 64 encoded and RSA encrypted string and returns the decrypted string.
    /// </summary>
    public class DecryptionTransformerAttribute : ValueTransformerAttributeBase
    {
        public override string TransformValue(string value, string valueName, PropertyInfo property)
        {
            return EncryptionUtility.Decrypt(value);
        }
    }
}
