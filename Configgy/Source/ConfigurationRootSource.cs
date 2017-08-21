using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Configgy.Source;
using Microsoft.Extensions.Configuration;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from any <see cref="IConfigurationRoot"/>.
    /// </summary>
    public class ConfigurationRootSource : ValueSourceAttributeBase
    {
        /// <summary>
        /// The <see cref="IConfigurationRoot"/> this <see cref="ConfigurationRootSource"/> uses to get values.
        /// </summary>
        public IConfigurationRoot ConfigurationRoot { get; }

        /// <summary>
        /// Constructs a <see cref="ConfigurationRootSource"/> using a default <see cref="IConfigurationRoot"/> that follows the same logic as the standard .NET Core MVC template.
        /// </summary>
        public ConfigurationRootSource()
        {
            const string environmentVariableName = "ASPNETCORE_ENVIRONMENT";

            var environment = Environment.GetEnvironmentVariable(environmentVariableName);

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
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
            // Get the prefix (if there is one)
            var prefix = (property as ICustomAttributeProvider)?.GetCustomAttributes(true).OfType<ConfigurationRootPrefixAttribute>().SingleOrDefault();

            // The initial section is the root configuration
            var section = ConfigurationRoot as IConfiguration;

            // If there is a prefix get the value from the section
            if (prefix?.Prefixes != null)
            {
                foreach (var p in prefix.Prefixes)
                {
                    section = section.GetSection(p);
                    if (section != null) continue;

                    value = null;
                    return false;
                }
            }

            // Get the value from the current 
            value = section[valueName];
            return value != null;
        }
    }
}