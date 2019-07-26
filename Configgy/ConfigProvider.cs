using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Exceptions;
using Configgy.Source;
using Configgy.Transformation;
using Configgy.Validation;

namespace Configgy
{
    /// <summary>
    /// A class that can populate any config object that doesn't inherit
    /// from <see cref="Config"/> using the same logic and modular configuration options.
    /// </summary>
    public class ConfigProvider : IConfigProvider
    {
        private static readonly Type StringType = typeof(string);

        private readonly ConcurrentDictionary<Type, Func<string, PropertyInfo, object>> getCache;
        
        /// <summary>
        /// The <see cref="IValueCache"/> in use by this configuration.
        /// </summary>
        public IValueCache Cache { get; }

        /// <summary>
        /// The <see cref="IValueSource"/> in use by this configuration.
        /// </summary>
        public IValueSource Source { get; }

        /// <summary>
        /// The <see cref="IValueTransformer"/> in use by this configuration.
        /// </summary>
        public IValueTransformer Transformer { get; }

        /// <summary>
        /// The <see cref="IValueValidator"/> in use by this configuration.
        /// </summary>
        public IValueValidator Validator { get; }

        /// <summary>
        /// The <see cref="IValueCoercer"/> in use by this configuration.
        /// </summary>
        public IValueCoercer Coercer { get; }

        /// <summary>
        /// Create a default ConfigPopulator instance.
        /// </summary>
        public ConfigProvider()
            : this(new DictionaryCache(), new AggregateSource(), new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer())
        {
        }

        /// <summary>
        /// Create a default ConfigPopulator instance and include parameters from a command line.
        /// </summary>
        /// <param name="commandLine">The command line to parse configuration values from.</param>
        public ConfigProvider(string[] commandLine)
            : this(new DictionaryCache(), new AggregateSource(commandLine), new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer())
        {
        }

        /// <summary>
        /// Create a totally customized ConfigPopulator instance.
        /// Generally you will not need this constructor.
        /// </summary>
        /// <param name="cache">The <see cref="IValueCache"/> instance to be used by this Config instance.</param>
        /// <param name="source">The <see cref="IValueSource"/> instance to be used by this Config instance.</param>
        /// <param name="transformer">The <see cref="IValueTransformer"/> instance to be used by this Config instance.</param>
        /// <param name="validator">The <see cref="IValueValidator"/> instance to be used by this Config instance.</param>
        /// <param name="coercer">The <see cref="IValueCoercer"/> instance to be used by this Config instance.</param>
        public ConfigProvider(IValueCache cache, IValueSource source, IValueTransformer transformer, IValueValidator validator, IValueCoercer coercer)
        {
            Cache = cache;
            Source = source;
            Transformer = transformer;
            Validator = validator;
            Coercer = coercer;
            
            getCache = new ConcurrentDictionary<Type, Func<string, PropertyInfo, object>>();
        }

        /// <summary>
        /// Clear all cached configuration values.
        /// </summary>
        public void ClearCache()
        {
            Cache.Clear();
        }

        /// <summary>
        /// Clear a single value from the cache by name.
        /// <param name="valueName">
        ///     The name of the value to remove from the cache.
        /// </param>
        /// </summary>
        public void ClearCache(string valueName)
        {
            Cache.Remove(valueName);
        }
        
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
        public T Get<T>(string valueName, PropertyInfo property)
        {
            return (T)Cache.Get(valueName, x => ProduceValue<T>(x, property));
        }

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
        public object Get(string valueName, PropertyInfo property, Type valueType)
        {
            var function = getCache.GetOrAdd(valueType, GetGet);
            return function(valueName, property);
        }
        
        private Func<string, PropertyInfo, object> GetGet (Type valueType)
        {
            var getPrototype = GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .OfType<MethodInfo>()
                .Where(x => x.Name == nameof(Get))
                .Single(m => m.IsGenericMethodDefinition);
            
            var get = getPrototype.MakeGenericMethod(valueType);
            
            return (s, p) => get.Invoke(this, new object[] {s, p});
        }
        
        private object ProduceValue<T>(string valueName, PropertyInfo property)
        {
            // Get the value from the factory
            if (!Source.Get(valueName, property, out var value))
            {
                // Throw an exception informing the user of the missing value
                throw new MissingValueException(valueName);
            }

            // Transform the value
            value = Transformer.Transform(value, valueName, property);

            // Validate the value
            var coerced = Validator.Validate(value, valueName, property, out T result);

            // Optimization: skip coercion for string values
            var type = typeof(T);
            if (type == StringType) return value;

            // Optimization: if the validator did the coercion the just return that value
            if (coerced) return result;

            // Coerce the value
            if (!Coercer.Coerce(value, valueName, property, out result))
            {
                // Throw an exception informing the user of the failed coercion
                throw new CoercionException(value, valueName, type, property);
            }

            // Return the result
            return result;
        }
    }
}