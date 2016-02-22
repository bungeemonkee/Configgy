using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Configgy.Source
{
    public class DefaultValueAttributeSource : IValueSource
    {
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            if (property == null) return null;

            return property
                .GetCustomAttributes<DefaultValueAttribute>()
                .Select(a => a.Value as string)
                .FirstOrDefault();
        }
    }
}
