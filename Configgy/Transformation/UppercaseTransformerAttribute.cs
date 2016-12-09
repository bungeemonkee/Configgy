using System.Globalization;
using System.Reflection;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that makes a string upper case.
    /// </summary>
    public class UppercaseTransformerAttribute : ValueTransformerAttributeBase
    {
#if !NETSTANDARD1_3
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
#endif

        public override string Transform(string value, string valueName, PropertyInfo property)
        {
#if NETSTANDARD1_3
            return value?.ToUpper();
#else
            return value?.ToUpper(Culture);
#endif
        }
    }
}
