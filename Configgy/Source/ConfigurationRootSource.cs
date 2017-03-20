using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from any <see cref="IConfigurationRoot"/>.
    /// </summary>
    public class ConfigurationRootSource : ValueSourceAttributeBase
    {
        public IConfigurationRoot ConfigurationRoot { get; }

        /// <summary>
        /// Constructs a <see cref="ConfigurationRootSource"/> using a default <see cref="IConfigurationRoot"/> that follows the same logic as the standard .NET Core MVC template.
        /// </summary>
        public ConfigurationRootSource()
        {
            const string environmentVariableName = "ASPNETCORE_ENVIRONMENT";

            var environment = Environment.GetEnvironmentVariable(environmentVariableName);

            var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            ConfigurationRoot = builder.Build();
        }

        /// <summary>
        /// Constructs a <see cref="ConfigurationRootSource"/> using the given <see cref="IConfigurationRoot"/>.
        /// </summary>
        /// <param name="configurationRoot">The <see cref="IConfigurationRoot"/> to use when searching for configuration values.</param>
        public ConfigurationRootSource(IConfigurationRoot configurationRoot)
        {
            ConfigurationRoot = configurationRoot;
        }

        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <param name="value">The value found in the source.</param>
        /// <returns>True if the config value was found in the source, false otherwise.</returns>
        public override bool Get(string valueName, PropertyInfo property, out string value)
        {
            try
            {
                value = ConfigurationRoot[valueName];
            }
            catch
            {
                value = null;
                return false;
            }

            return true;
        }
    }
}