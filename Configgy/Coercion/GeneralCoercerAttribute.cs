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
        /// <returns>The coerced value or null if the value could not be coerced.</returns>
        public override object CoerceTo<T>(string value, string valueName, PropertyInfo property)
        {
            var type = typeof(T);

            var converter = TypeDescriptor.GetConverter(type);
            if (converter == null) return null;

            return converter.ConvertFromString(value);
        }
    }
}
