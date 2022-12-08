using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Configgy.Coercion
{
    /// <summary>
    /// A Value coercer that converts a JSON string into an object.
    /// </summary>
    public class JsonCoercerAttribute : ValueCoercerAttributeBase
    {
        private static readonly DataContractJsonSerializerSettings Settings = new()
        {
            UseSimpleDictionaryFormat = true
        };

        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>
        public override bool Coerce<T>(IConfigProperty property, string? value, out T result)
        {
            // If the value is null
            if (value == null)
            {
                // Set the result to the default for the type
                result = default!;

                // If the value is nullable we can (and did) coerce it, otherwise we can't
                return IsNullable<T>();
            }

            // Deserialize the JSON data to an object
            var serializer = new DataContractJsonSerializer(typeof(T), Settings);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
            result = (T)serializer.ReadObject(stream)!;
            return true;
        }
    }
}
