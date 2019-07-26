using Configgy.Cache;
using Configgy.Source;
using Configgy.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Configgy.Validation;
using Configgy.Coercion;
using System;
using Configgy.Transformation;

namespace Configgy
{
    /// <summary>
    /// The main Configgy configuration base class.
    /// </summary>
    public abstract class Config
    {
        private readonly IReadOnlyDictionary<string, PropertyInfo> _properties;

        /// <summary>
        /// The <see cref="IValueCache"/> in use by this configuration.
        /// </summary>
        [Obsolete("This property will likely be removed in the next major version.", false)]
        protected IValueCache Cache => Provider.Cache;

        /// <summary>
        /// The <see cref="IValueSource"/> in use by this configuration.
        /// </summary>
        [Obsolete("This property will likely be removed in the next major version.", false)]
        protected IValueSource Source => Provider.Source;

        /// <summary>
        /// The <see cref="IValueTransformer"/> in use by this configuration.
        /// </summary>
        [Obsolete("This property will likely be removed in the next major version.", false)]
        protected IValueTransformer Transformer => Provider.Transformer;

        /// <summary>
        /// The <see cref="IValueValidator"/> in use by this configuration.
        /// </summary>
        [Obsolete("This property will likely be removed in the next major version.", false)]
        protected IValueValidator Validator => Provider.Validator;

        /// <summary>
        /// The <see cref="IValueCoercer"/> in use by this configuration.
        /// </summary>
        [Obsolete("This property will likely be removed in the next major version.", false)]
        protected IValueCoercer Coercer => Provider.Coercer;
        
        /// <summary>
        /// The <see cref="IConfigProvider"/> in use by this configuration.
        /// </summary>
        protected IConfigProvider Provider { get; }

        /// <summary>
        /// Create a default Config instance.
        /// </summary>
        protected Config()
            : this(new ConfigProvider(new DictionaryCache(), new AggregateSource(), new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer()))
        {
        }

        /// <summary>
        /// Create a default Config instance and include parameters from a command line.
        /// </summary>
        /// <param name="commandLine">The command line to parse configuration values from.</param>
        protected Config(string[] commandLine)
            : this(new ConfigProvider(new DictionaryCache(), new AggregateSource(commandLine), new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer()))
        {
        }

        /// <summary>
        /// Create a totally customized Config instance.
        /// Generally you will not need this constructor.
        /// </summary>
        /// <param name="cache">The <see cref="IValueCache"/> instance to be used by this Config instance.</param>
        /// <param name="source">The <see cref="IValueSource"/> instance to be used by this Config instance.</param>
        /// <param name="transformer">The <see cref="IValueTransformer"/> instance to be used by this Config instance.</param>
        /// <param name="validator">The <see cref="IValueValidator"/> instance to be used by this Config instance.</param>
        /// <param name="coercer">The <see cref="IValueCoercer"/> instance to be used by this Config instance.</param>
        [Obsolete("This constructor will likely be removed in the next major version.", false)]
        protected Config(IValueCache cache, IValueSource source, IValueTransformer transformer, IValueValidator validator, IValueCoercer coercer)
            : this(new ConfigProvider(cache, source, transformer, validator, coercer))
        {
        }

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
        /// Clear all cached configuration values.
        /// </summary>
        [Obsolete("This method will likely be removed in the next major version.", false)]
        public void ClearCache()
        {
            Cache.Clear();
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
