using System.ComponentModel;
using System.Linq;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from <see cref="DefaultValueAttribute"/> instances on a <see cref="Config"/> property.
    /// </summary>
    public class DefaultValueAttributeSource : ValueSourceAttributeBase
    {
        /// <inheritdoc cref="IValueSource.Get"/>
        public override bool Get(IConfigProperty property, out string? value)
        {
            // Get the default value attribute
            var attribute = property.Attributes
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
