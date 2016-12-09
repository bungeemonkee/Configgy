#if !NETSTANDARD1_3
using System.Configuration;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from the appSettings element of the app/web config file.
    /// </summary>
    public class AppSettingSource : IValueSource
    {
        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <param name="value">The value found in the source.</param>
        /// <returns>True if the config value was found in the source, false otherwise.</returns>
        public bool Get(string valueName, PropertyInfo property, out string value)
        {
            value = ConfigurationManager.AppSettings[valueName];
            return value != null;
        }
    }
}
#endif