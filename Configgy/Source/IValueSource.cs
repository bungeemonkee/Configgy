namespace Configgy.Source
{
    /// <summary>ww
    /// Defines a source of configuration values.
    /// </summary>
    public interface IValueSource
    {
        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="property">The <see cref="IConfigProperty"/> for this value.</param>
        /// <param name="value">The value found in the source.</param>
        /// <returns>True if the config value was found in the source, false otherwise.</returns>
        bool Get(IConfigProperty property, out string? value);
    }
}
