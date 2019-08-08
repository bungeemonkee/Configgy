using System;

namespace Configgy.Coercion
{
    /// <summary>
    /// A value coercer that converts a base 64 encoded string into a byte array.
    /// </summary>
    public class Base64EncodedCoercerAttribute : ValueCoercerAttributeBase
    {
        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>
        public override bool Coerce<T>(IConfigProperty property, string value, out T result)
        {
            // Only try to coerce values into byte arrays
            if (typeof(T) != typeof(byte[]))
            {
                result = default;
                return false;
            }

            // If the value is null, return null
            if (value == null)
            {
                result = default;
                return true;
            }

            // Convert form base 64 to a byte array
            result = (T)(object)Convert.FromBase64String(value);
            return true;
        }
    }
}
