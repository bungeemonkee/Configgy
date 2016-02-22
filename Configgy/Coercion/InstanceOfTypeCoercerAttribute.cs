using System;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A coercer that creates instances of the given type name.
    /// </summary>
    /// <remarks>
    /// See the documentation for <see cref="Type.GetType(string, bool, bool)"/> and <see cref="Activator.CreateInstance(Type)"/>.
    /// </remarks>
    public class InstanceOfTypeCoercerAttribute : ValueCoercerAttributeBase, IValueCoercer
    {
        public override object CoerceTo<T>(string value, string valueName, PropertyInfo property)
        {
            var type = Type.GetType(value, false, true);
            if (type == null) return null;

            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }
    }
}
