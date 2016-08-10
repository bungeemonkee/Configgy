using System.Globalization;
using System.Reflection;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that makes a string upper case.
    /// </summary>
    public class UppercaseTransformerAttribute : ValueTransformerAttributeBase
    {
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        public override string Transform(string value, string valueName, PropertyInfo property)
        {
            return value?.ToUpper(Culture);
        }
    }
}
