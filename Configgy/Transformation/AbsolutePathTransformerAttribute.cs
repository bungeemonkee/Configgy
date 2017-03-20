using System.IO;
using System.Reflection;

namespace Configgy.Transformation
{
    /// <summary>
    /// A value transformer that converts a relative file path to a full file path.
    /// </summary>
    public class AbsolutePathTransformerAttribute : ValueTransformerAttributeBase
    {
        public override string Transform(string value, string valueName, ICustomAttributeProvider property)
        {
            return value == null
                ? null
                : Path.GetFullPath(value);
        }
    }
}
