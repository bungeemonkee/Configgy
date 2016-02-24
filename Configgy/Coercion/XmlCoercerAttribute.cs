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
