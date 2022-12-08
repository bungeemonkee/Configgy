using System;
using System.ComponentModel;
using System.Linq;

namespace Configgy.Coercion
{
    /// <summary>
    /// An <see cref="IValueCoercer"/> that can convert a comma separated set of values into an array.
    /// </summary>
    public class CsvCoercerAttribute : ValueCoercerAttributeBase
    {
        private readonly TypeConverter _converter;

        /// <summary>
        /// The type of the items in the array.
        /// </summary>
        public readonly Type ItemType;

        /// <summary>
        /// The type of the array that contains the items.
        /// </summary>
        public readonly Type ArrayType;

        /// <summary>
        /// The string that separates the individual values.
        /// </summary>
        public readonly string Separator;

        /// <summary>
        /// Creates a CSVCoercerAttribute using the given item type and a comma as the separator.
        /// </summary>
        /// <param name="itemType">The type of the items in the array.</param>
        public CsvCoercerAttribute(Type itemType)
            : this(itemType, ",")
        {
        }

        /// <summary>
        /// Creates a CSVCoercerAttribute using the given item type and separator.
        /// </summary>
        /// <param name="itemType">The type of the items in the array.</param>
        /// <param name="separator">The string that separates the individual values.</param>
        public CsvCoercerAttribute(Type itemType, string separator)
        {
            ItemType = itemType;
            ArrayType = itemType.MakeArrayType();
            Separator = separator;

            // get the converter for the given item type
            _converter = TypeDescriptor.GetConverter(ItemType);
        }

        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>
        public override bool Coerce<T>(IConfigProperty property, string? value, out T result)
        {
            // make sure the requested type is actually correct
            if (typeof(T) != ArrayType)
            {
                result = default!;
                return false;
            }

            // Make sure the onverter can actually do the conversion
            if (!_converter.CanConvertFrom(typeof(string)))
            {
                throw new InvalidOperationException($"Unable to convert type {ItemType} from string.");
            }

            // If the string is null then return null
            if (value == null)
            {
                result = default!;
                return true;
            }

            // If the string is empty then just return an empty array
            if (value == string.Empty)
            {
                result = (T)(object)Array.CreateInstance(ItemType, 0);
                return true;
            }

            // get the converted values
            var values = value.Split(new[] { Separator }, StringSplitOptions.None)
                .Select(x => _converter.ConvertFromString(x))
                .ToArray();

            // create the properly typed result array
            result = (T)(object)Array.CreateInstance(ItemType, values.Length);

            // copy the converted values into the typed result array
            Array.Copy(values, (Array)(object)result!, values.Length);

            // return the result array
            return true;
        }
    }
}
