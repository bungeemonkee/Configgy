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
        /// <param name="result">The coerced value.</param>
        /// <returns>True if the value could be coerced, false otherwise.</returns>
        public override bool Coerce<T>(string value, string valueName, ICustomAttributeProvider property, out T result)
        {
            // If the value is null
            if (value == null)
            {
                // Set the result to the default for the type
                result = default(T);

                // If the value is nullable we can (and did) coerce it, otherwise we can't
                return IsNullable<T>();
            }

            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                result = (T)serializer.ReadObject(stream);
                return true;
            }
        }
    }
}
