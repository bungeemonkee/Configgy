using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// An <see cref="IValueCoercer"/> that can convert a comma separated set of values into an array.
    /// </summary>
    public class CsvCoercerAttribute : ValueCoercerAttributeBase
    {
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
        }

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
            // make sure the requested type is actually correct
            if (typeof(T) != ArrayType) return null;

            // get the converter for the given item type
            var converter = TypeDescriptor.GetConverter(ItemType);

            try
            {
                // get the converted values
                var values = value.Split(new string[] { Separator }, StringSplitOptions.None)
                    .Select(x => converter.ConvertFromString(x))
                    .ToArray();

                // create the properly typed result array
                var result = Array.CreateInstance(ItemType, values.Length);

                // copy the converted values into the typed result array
                Array.Copy(values, result, values.Length);

                // return the result array
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
