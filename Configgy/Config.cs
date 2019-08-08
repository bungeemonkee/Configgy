using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Configgy
{
    /// <summary>
    /// The main Configgy configuration base class.
    /// </summary>
    public abstract class Config
    {
        private readonly IReadOnlyDictionary<string, PropertyInfo> _properties;

        /// <summary>
        /// The <see cref="IConfigProvider"/> in use by this configuration.
        /// </summary>
        public IConfigProvider Provider { get; }

        /// <summary>
        /// Create a Config instance using the given <see cref="IConfigProvider"/>.
        /// </summary>
        protected Config(IConfigProvider provider)
        {
            Provider = provider;

            // Pre-cache all the properties on this instance
            _properties = GetType()
                .GetProperties()
                .ToDictionary(p => p.Name);
        }

        /// <summary>
        /// Get a configuration value.
        /// </summary>
        /// <typeparam name="T">The type of the expected configuration value.</typeparam>
        /// <param name="valueName">
        ///     The name of the configuration value to get.
        ///     This will automatically be the name of the calling method or property and will be populated by the compiler.
        ///     The may be given an explicit value if the property name and setting name do not match.
        /// </param>
        /// <param name="propertyName">
        ///     The name of the property on the configuration object.
        ///     This will automatically be the name of the calling method or property and will be populated by the compiler.
        ///     Do not pass a value to this paramater so that the compiler default (the property name) is preserved.
        /// </param>
        /// <returns>The configuration value.</returns>
        protected T Get<T>([CallerMemberName] string valueName = null, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null || !_properties.TryGetValue(propertyName, out var property))
            {
                property = null;
            }

            return Provider.Get<T>(valueName, property);
        }
    }
}
