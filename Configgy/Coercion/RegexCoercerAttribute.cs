using System.Reflection;
using System.Text.RegularExpressions;

namespace Configgy.Coercion
{
    /// <summary>
    /// A coercer that creates instances of <see cref="Regex"/> from strings.
    /// </summary>
    /// <remarks>
    /// See the documentation for <see cref="Regex"/>.
    /// </remarks>
    public class RegexCoercerAttribute : ValueCoercerAttributeBase, IValueCoercer
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
            // Only try to coerce values into Regex objects
            if (typeof(T) != typeof(Regex)) return null;

            return new Regex(value);
        }
    }
}
