using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Configgy.Coercion
{
    /// <summary>
    /// A Value coercer that converts an XML string into an object.
    /// </summary>
    public class XmlCoercerAttribute : ValueCoercerAttributeBase
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
            try
            {
                var serializer = new DataContractSerializer(typeof(T));

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                {
                    return serializer.ReadObject(stream);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
