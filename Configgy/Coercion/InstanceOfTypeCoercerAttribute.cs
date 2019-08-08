using System;

namespace Configgy.Coercion
{
    /// <summary>
    /// A coercer that creates instances of the given type name.
    /// </summary>
    /// <remarks>
    /// See the documentation for <see cref="Type.GetType(string, bool, bool)"/> and <see cref="Activator.CreateInstance(Type)"/>.
    /// </remarks>
    public class InstanceOfTypeCoercerAttribute : ValueCoercerAttributeBase
    {
        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>s
        public override bool Coerce<T>(IConfigProperty property, string value, out T result)
        {
            var type = Type.GetType(value, false, true);
            if (type == null)
            {
                result = default;
                return false;
            }

            result = (T)Activator.CreateInstance(type);
            return true;
        }
    }
}
