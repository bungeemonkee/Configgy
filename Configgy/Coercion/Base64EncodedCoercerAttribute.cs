using System;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A value coercer that converts a base 64 encoded string into a byte array.
    /// </summary>
    public class Base64EncodedCoercerAttribute : ValueCoercerAttributeBase
    {
        /// <summary>
        /// Coerce the raw string value into the expected result type.
        /// </summary>
        /// <typeparam name="T">The expected result type after coercion.</typeparam>
        /// <param name="value">The raw string value to be coerced.</param>
        /// <param name="valueName">The name of the value to be coerced.</param>
        /// <param name="property">If this value is directly associated with a property on a <see cref="Config"/> instance this is the reference to that property.</param>
        /// <param name="result">The coerced value.</param>
        /// <returns>True if the value could be coerced, false otherwise.</returns>
        public override bool Coerce<T>(string value, string valueName, ICustomAttributeProvider property, out T result)
        {
            // Only try to coerce values into byte arrays
            if (typeof(T) != typeof(byte[]))
            {
                result = default(T);
                return false;
            }

            // If the value is null, return null
            if (value == null)
            {
                result = default(T);
                return true;
            }

            // Convert form base 64 to a byte array
            result = (T)(object)Convert.FromBase64String(value);
            return true;
        }
    }
}
