using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from <see cref="DefaultValueAttribute"/> instances on a <see cref="Config"/> property.
    /// </summary>
    public class DefaultValueAttributeSource : IValueSource
    {
        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <returns>The raw configuration value or null if there isn't one in this source.</returns>
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            if (property == null) return null;

            return property
                .GetCustomAttributes(true)
                .OfType<DefaultValueAttribute>()
                .Select(a => a.Value as string)
                .Where(v => !string.IsNullOrEmpty(v))
                .FirstOrDefault();
        }
    }
}
