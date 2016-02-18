using System.ComponentModel;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A general purpose value coercer that operates using the <see cref="TypeConverter"/> instances from <see cref="TypeDescriptor"/>.
    /// </summary>
    public class GeneralCoercer : IValueCoercer
    {
        public object CoerceTo<T>(string value, string valueName, PropertyInfo property)
        {
            var type = typeof(T);

            var converter = TypeDescriptor.GetConverter(type);
            if (converter == null) return null;

            return converter.ConvertFromString(value);
        }
    }
}
