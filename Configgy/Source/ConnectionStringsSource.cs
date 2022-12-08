using System.Configuration;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from the connectionStrings element of the app/web config file.
    /// </summary>
    public class ConnectionStringsSource : ValueSourceAttributeBase
    {
        /// <inheritdoc cref="ValueSourceAttributeBase.Get"/>
        public override bool Get(IConfigProperty property, out string? value)
        {
            value = ConfigurationManager.ConnectionStrings[property.ValueName]?.ConnectionString;
            return value != null;
        }
    }
}
