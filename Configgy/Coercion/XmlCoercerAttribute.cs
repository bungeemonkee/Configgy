using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

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
                var serializer = new XmlSerializer(typeof(T));
                using (var stream = new StringReader(value))
                using (var reader = XmlReader.Create(stream))
                {
                    return serializer.Deserialize(reader);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
