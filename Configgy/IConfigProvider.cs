using System;
using System.Reflection;
using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Transformation;
using Configgy.Validation;

namespace Configgy
{
    /// <summary>
    /// Defines a way to populate any config object that doesn't inherit
    /// from <see cref="Config"/> using the same logic and modular configuration options.
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// The <see cref="IValueCache"/> in use by this configuration.
        /// </summary>
        IValueCache Cache { get; }

        /// <summary>
        /// The <see cref="IValueSource"/> in use by this configuration.
        /// </summary>
        IValueSource Source { get; }

        /// <summary>
        /// The <see cref="IValueTransformer"/> in use by this configuration.
        /// </summary>
        IValueTransformer Transformer { get; }

        /// <summary>
        /// The <see cref="IValueValidator"/> in use by this configuration.
        /// </summary>
        IValueValidator Validator { get; }

        /// <summary>
        /// The <see cref="IValueCoercer"/> in use by this configuration.
        /// </summary>
        IValueCoercer Coercer { get; }

        /// <summary>
        /// Clear all cached configuration values.
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Clear a single value from the cache by name.
        /// <param name="valueName">
        ///     The name of the value to remove from the cache.
        /// </param>
        /// </summary>
        void ClearCache(string valueName);

        /// <summary>
        /// Get a configuration value.
        /// </summary>
        /// <typeparam name="T">The type of the expected configuration value.</typeparam>
        /// <param name="valueName">
        ///     The name of the configuration value to get.
        /// </param>
        /// <param name="property">
        ///     The <see cref="PropertyInfo"/> for the property being populated.
        /// </param>
        /// <returns>The configuration value.</returns>
        T Get<T>(string valueName, PropertyInfo property);

        /// <summary>
        /// Get a configuration value.
        /// </summary>
        /// <param name="valueName">
        ///     The name of the configuration value to get.
        /// </param>
        /// <param name="property">
        ///     The <see cref="PropertyInfo"/> for the property being populated.
        /// </param>
        /// <param name="valueType">
        ///     The <see cref="Type"/> of the value to be returned.
        /// </param>
        /// <returns>The configuration value.</returns>
        object Get(string valueName, PropertyInfo property, Type valueType);
    }
}