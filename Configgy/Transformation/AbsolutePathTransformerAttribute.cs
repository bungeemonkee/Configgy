using System.IO;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that converts a relative file path to a full file path.
    /// </summary>
    public class AbsolutePathTransformerAttribute : ValueTransformerAttributeBase
    {
        /// <inheritdoc cref="IValueTransformer.Transform"/>
        public override string Transform(IConfigProperty property, string value)
        {
            return value == null
                ? null
                : Path.GetFullPath(value);
        }
    }
}
