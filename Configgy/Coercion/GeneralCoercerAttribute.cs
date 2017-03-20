using System;
using System.ComponentModel;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A general purpose value coercer that operates using the <see cref="TypeConverter"/> instances from <see cref="TypeDescriptor"/>.
    /// </summary>
    public class GeneralCoercerAttribute : ValueCoercerAttributeBase, IValueCoercer
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
            var type = typeof(T);

            // If the value is null...
            if (value == null)
            {
                // Set the result to the default
                result = default(T);

                // If the type is nullable return true, if not then the default value is not correct
                return IsNullable<T>();
            }

            var converter = TypeDescriptor.GetConverter(type);

            // If the converter can't convert this type of thing then don't try
            if (!converter.CanConvertFrom(typeof(string)))
            {
                result = default(T);
                return false;
            }

            // Convert the value
            result = (T)converter.ConvertFromString(value);
            return true;
        }
    }
}
