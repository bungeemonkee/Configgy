using Newtonsoft.Json;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A Value coercer that converts a JSON string into an object.
    /// </summary>
    public class JsonCoercerAttribute : ValueCoercerAttributeBase
    {
        public override object CoerceTo<T>(string value, string valueName, PropertyInfo property)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return null;
            }
        }
    }
}
