using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Configgy.Coercion
{
    /// <summary>
    /// A Value coercer that converts a JSON string into an object.
    /// </summary>
    public class JsonCoercerAttribute : ValueCoercerAttributeBase
    {
        private static readonly DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings()
        {
            UseSimpleDictionaryFormat = true
        };

        public override object CoerceTo<T>(string value, string valueName, PropertyInfo property)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T), settings);

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
