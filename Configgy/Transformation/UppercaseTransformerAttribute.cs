using System.Globalization;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that makes a string upper case.
    /// </summary>
    public class UppercaseTransformerAttribute : ValueTransformerAttributeBase
    {
        /// <summary>
        /// The <see cref="CultureInfo"/> used when changing the string.
        /// </summary>
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        /// <inheritdoc cref="IValueTransformer.Transform"/>
        public override string? Transform(IConfigProperty property, string? value)
        {
            return value != null
                ? Culture.TextInfo.ToUpper(value)
                : null;
        }
    }
}
