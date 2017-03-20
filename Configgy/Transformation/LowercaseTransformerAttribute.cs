using System.Globalization;
using System.Reflection;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that makes a string lower case.
    /// </summary>
    public class LowercaseTransformerAttribute : ValueTransformerAttributeBase
    {
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        public override string Transform(string value, string valueName, ICustomAttributeProvider property)
        {
            return value != null
                ? Culture.TextInfo.ToLower(value)
                : null;
        }
    }
}
