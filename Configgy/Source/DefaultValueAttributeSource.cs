using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from <see cref="DefaultValueAttribute"/> instances on a <see cref="Config"/> property.
    /// </summary>
    public class DefaultValueAttributeSource : ValueSourceAttributeBase
    {
        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <param name="value">The value found in the source.</param>
        /// <returns>True if the config value was found in the source, false otherwise.</returns>
        public override bool Get(string valueName, PropertyInfo property, out string value)
        {
            // Get the default value attribute
            var attribute = ((ICustomAttributeProvider)property)?.GetCustomAttributes(true)
                .OfType<DefaultValueAttribute>()
                .SingleOrDefault();

            // If there is no attribute return false
            if (attribute == null)
            {
                value = null;
                return false;
            }

            // If the value is null handle that explicitly
            if (attribute.Value == null)
            {
                value = null;
                return true;
            }

            // Convert the value to a string and return true
            value = attribute.Value as string ?? attribute.Value.ToString();
            return true;
        }
    }
}
