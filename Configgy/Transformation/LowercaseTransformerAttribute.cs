using System.Globalization;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that makes a string lower case.
    /// </summary>
    public class LowercaseTransformerAttribute : ValueTransformerAttributeBase
    {
        /// <summary>
        /// The <see cref="CultureInfo"/> used when changing the string.
        /// </summary>
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        /// <inheritdoc cref="IValueTransformer.Transform"/>
        public override string Transform(IConfigProperty property, string value)
        {
            return value != null
                ? Culture.TextInfo.ToLower(value)
                : null;
        }
    }
}
