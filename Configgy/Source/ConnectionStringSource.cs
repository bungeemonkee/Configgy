using System.Configuration;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from the connectionStrings element of the app/web config file.
    /// </summary>
    public class ConectionStringsSource : IValueSource
    {
        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <returns>The raw configuration value or null if there isn't one in this source.</returns>
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[valueName];
            if (connectionString == null) return null;
            return connectionString.ConnectionString;
        }
    }
}
