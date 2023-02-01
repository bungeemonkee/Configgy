using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private readonly IDictionary<string, IList<object>> _attributeCache;
        private readonly ConcurrentDictionary<Type, Func<string, PropertyInfo, object>> _getCache;
        
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
            
            _attributeCache = new Dictionary<string, IList<object>>();
            _getCache = new ConcurrentDictionary<Type, Func<string, PropertyInfo, object>>();
        }

        /// <inheritdoc cref="IConfigProvider.AddAttribute"/>
        public void AddAttribute(string valueName, object attribute)
        {
            if (_attributeCache.TryGetValue(valueName, out var attributes))
            {
                attributes.Add(attribute);
            }
            else
            {
                attributes = new List<object>
                {
                    attribute
                };
                _attributeCache.Add(valueName, attributes);
            }
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
            return (T)Cache.Get(valueName, x => ProduceValue<T>(x, property)!)!;
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
            var function = _getCache.GetOrAdd(valueType, GetGet);
            return function(valueName, property);
        }
        
        private Func<string, PropertyInfo, object> GetGet (Type valueType)
        {
            var getPrototype = GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.Name == nameof(Get))
                .Single(m => m.IsGenericMethodDefinition);
            
            var get = getPrototype.MakeGenericMethod(valueType);
            
            return (s, p) => get.Invoke(this, new object[] {s, p})!;
        }
        
        private object? ProduceValue<T>(string valueName, PropertyInfo property)
        {
            // TODO: Inject the NullableReferenceChecker instance
            var nullableReferenceChecker = new NullableReferenceChecker();
            
            _attributeCache.TryGetValue(valueName, out var attributes);
            
            attributes = (attributes ?? Enumerable.Empty<object>())
                .Union(property.GetCustomAttributes(true))
                .ToArray();

            var names = attributes
                .OfType<AlternateNameAttribute>()
                .OrderBy(x => x.Priority)
                .Select(x => x.AlternateName)
                .Append(valueName);
            
            ConfigProperty prop = null!;
            string? value = null!;
            var found = false;
            
            // Try every known name in order
            foreach (var name in names)
            {
                prop = new ConfigProperty(name, property, attributes);
                
                if (Source.Get(prop, out value))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                if (nullableReferenceChecker.RequiresNullableReferenceCheck(prop))
                {
                    // Null reference checks for this property are required
                    // Make sure a null value is allowed
                    if (!nullableReferenceChecker.SatisfiesNullableReferenceRules(prop, null))
                    {
                        // This property requires nullable reference validation but failed it
                        throw new NullableReferenceValidationException(valueName);
                    }
                    
                    // No value was found but this property can be null so implicitly "find" a null value
                    found = true;
                    value = null;
                }
                else
                {
                    // No value was found but the property is either a value type or doesn't
                    // have nullability reference type annotations, so fall back on the old behavior
                    // Throw an exception informing the user of the missing value
                    throw new MissingValueException(valueName);
                }
            }

            // Transform the value
            value = Transformer.Transform(prop, value);

            // Validate the value
            var coerced = Validator.Validate(prop, value, out T result);

            // Optimization: skip coercion for string values
            var type = typeof(T);
            if (type == StringType) return value;

            // Optimization: if the validator did the coercion the just return that value
            if (coerced) return result;

            // Coerce the value
            if (!Coercer.Coerce(prop, value, out result))
            {
                // Throw an exception informing the user of the failed coercion
                throw new CoercionException(value, valueName, type, property);
            }

            // Check the value for nullability
            if (nullableReferenceChecker.RequiresNullableReferenceCheck(prop) && !nullableReferenceChecker.SatisfiesNullableReferenceRules(prop, value))
            {
                throw new NullableReferenceValidationException(valueName);
            }

            // Return the result
            return result;
        }
    }
}