using System;

namespace Configgy.Coercion
{
    /// <summary>
    /// A coercer that creates instances of <see cref="Type"/> from type names.
    /// </summary>
    /// <remarks>
    /// See the documentation for <see cref="Type.GetType(string, bool, bool)"/>.
    /// </remarks>
    public class TypeCoercerAttribute : ValueCoercerAttributeBase, IValueCoercer
    {
        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>
        public override bool Coerce<T>(IConfigProperty property, string? value, out T result)
        {
            // If there is no type name there is no type to get
            if (value == null)
            {
                result = default!;
                return false;
            }
            
            // Only try to coerce values into Type objects
            if (typeof(T) != typeof(Type))
            {
                result = default!;
                return false;
            }

            // Get the type
            var type = Type.GetType(value, false, true);

            // Unable to get the type
            if (type == null)
            {
                result = default!;
                return false;
            }

            // Return the type
            result = (T)(object)type;
            return true;
        }
    }
}
