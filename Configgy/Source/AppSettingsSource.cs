using System.Configuration;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from the appSettings element of the app/web config file.
    /// </summary>
    public class AppSettingSource : ValueSourceAttributeBase
    {
        /// <inheritdoc cref="ValueSourceAttributeBase.Get"/>
        public override bool Get(IConfigProperty property, out string value)
        {
            value = ConfigurationManager.AppSettings[property.ValueName];
            return value != null;
        }
    }
}
