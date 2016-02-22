using System;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A coercer that creates instances of <see cref="Type"/> from type names.
    /// </summary>
    /// <remarks>
    /// See the documentation for <see cref="Type.GetType(string, bool, bool)"/>.
    /// </remarks>
    public class TypeCoercer : IValueCoercer
    {
        public object CoerceTo<T>(string value, string valueName, PropertyInfo property)
        {
            return Type.GetType(value, false, true);
        }
    }
}
