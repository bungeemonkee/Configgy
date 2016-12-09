using System.Globalization;
using System.Reflection;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that makes a string lower case.
    /// </summary>
    public class LowercaseTransformerAttribute : ValueTransformerAttributeBase
    {
#if !NETSTANDARD1_3
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
#endif

        public override string Transform(string value, string valueName, PropertyInfo property)
        {
#if NETSTANDARD1_3
            return value?.ToLower();
#else
            return value?.ToLower(Culture);
#endif
        }
    }
}
