
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// Defines a source of configuration values.
    /// </summary>
    public interface IValueSource
    {
        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the Config object that matches the requested value name then this will contain the reference to that property.</param>
        /// <returns>The raw configuration value or null if there isn't one in this source.</returns>
        string GetRawValue(string valueName, PropertyInfo property);
    }
}
