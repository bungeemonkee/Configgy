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
        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>
        public override bool Coerce<T>(IConfigProperty property, string value, out T result)
        {
            // Only try to coerce values into Regex objects
            if (typeof(T) != typeof(Regex))
            {
                result = default;
                return false;
            }

            // Create the regex
            result = (T)(object)new Regex(value);
            return true;
        }
    }
}
