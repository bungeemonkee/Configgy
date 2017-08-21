using System;
using Microsoft.Extensions.Configuration;

namespace Configgy.Source
{
    /// <summary>
    /// Defines the prefix used to search an <see cref="IConfigurationRoot"/> for a given configuration value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class ConfigurationRootPrefixAttribute : Attribute
    {
        /// <summary>
        /// The prefixes used to search an <see cref="IConfigurationRoot"/> for a given configuration value.
        /// </summary>
        public string[] Prefixes { get; }

        /// <summary>
        /// Create a new <see cref="ConfigurationRootPrefixAttribute"/> with the given prefix.
        /// </summary>
        /// <param name="prefixes">The prefix(es) used. The string is split on '.' to get a list of prefixes (for nested sections)</param>
        public ConfigurationRootPrefixAttribute(string prefixes)
            : this(prefixes, ".")
        {
        }

        /// <summary>
        /// Create a new <see cref="ConfigurationRootPrefixAttribute"/> with the given prefix.
        /// </summary>
        /// <param name="prefixes">The prefix(es) used.</param>
        /// <param name="seperator">The string used to split the </param>
        public ConfigurationRootPrefixAttribute(string prefixes, string seperator)
        {
            Prefixes = prefixes.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
