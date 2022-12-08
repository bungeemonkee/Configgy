using System.ComponentModel;

namespace Configgy.Coercion
{
    /// <summary>
    /// A general purpose value coercer that operates using the <see cref="TypeConverter"/> instances from <see cref="TypeDescriptor"/>.
    /// </summary>
    public class GeneralCoercerAttribute : ValueCoercerAttributeBase
    {
        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>
        public override bool Coerce<T>(IConfigProperty property, string? value, out T result)
        {
            var type = typeof(T);

            // If the value is null...
            if (value == null)
            {
                // Set the result to the default
                result = default!;

                // If the type is nullable return true, if not then the default value is not correct
                return IsNullable<T>();
            }

            var converter = TypeDescriptor.GetConverter(type);

            // If the converter can't convert this type of thing then don't try
            if (!converter.CanConvertFrom(typeof(string)))
            {
                result = default!;
                return false;
            }

            // Convert the value
            result = (T)converter.ConvertFromString(value)!;
            return true;
        }
    }
}
