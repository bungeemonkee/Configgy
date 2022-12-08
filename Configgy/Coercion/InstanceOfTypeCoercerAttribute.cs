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
        public override bool Coerce<T>(IConfigProperty property, string? value, out T result)
        {
            // If there is no type name there is no type to get
            if (value == null)
            {
                result = default!;
                return false;
            }
            
            // Get the type
            var type = Type.GetType(value, false, true);
            
            // The type could not be found
            if (type == null)
            {
                result = default!;
                return false;
            }

            // Create the instance
            result = (T)Activator.CreateInstance(type)!;
            return true;
        }
    }
}
