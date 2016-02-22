using Configgy.Cache;
using Configgy.Source;
using Configgy.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Configgy.Validation;
using Configgy.Coercion;

namespace Configgy
{
    /// <summary>
    /// The main Configgy configuration base class.
    /// </summary>
    public abstract class Config
    {
        private readonly IValueCache _cache;
        private readonly IValueSource _source;
        private readonly IValueValidator _validator;
        private readonly IValueCoercer _coercer;
        private readonly IReadOnlyDictionary<string, PropertyInfo> _properties;

        /// <summary>
        /// Create a default Config instance.
        /// </summary>
        protected Config()
            : this(new DictionaryCache(), new AggregateSource(), new AggregateValidator(), new AggregateCoercer())
        {
        }

        /// <summary>
        /// Create a default Config instance and include parameters from a command line.
        /// </summary>
        /// <param name="commandLine">The command line to parse configuration values from.</param>
        protected Config(string[] commandLine)
            : this(new DictionaryCache(), new AggregateSource(commandLine), new AggregateValidator(), new AggregateCoercer())
        {
        }

        /// <summary>
        /// Create a totally customized Config instance.
        /// Generally you will not need this constructor.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="valueFactory"></param>
        protected Config(IValueCache cache, IValueSource source, IValueValidator validator, IValueCoercer coercer)
        {
            _cache = cache;
            _source = source;
            _validator = validator;
            _coercer = coercer;

            _properties = GetType()
                .GetMembers(BindingFlags.Instance | BindingFlags.Public)
                .OfType<PropertyInfo>()
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name);
        }

        /// <summary>
        /// Clear all cached configuration values.
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Get a configuration value.
        /// </summary>
        /// <typeparam name="T">The type of the expected configuration value.</typeparam>
        /// <param name="valueName">
        ///     The name of the configuration value to get.
        ///     This will automatically be the name of the calling method or property and will be populated by the compiler.
        /// </param>
        /// <returns>The configuration value.</returns>
        protected T Get<T>([CallerMemberName] string valueName = null)
        {
            return (T)_cache.GetValue(valueName, ProduceValue<T>);
        }

        private object ProduceValue<T>(string valueName)
        {
            // get the property reference
            PropertyInfo property;
            _properties.TryGetValue(valueName, out property);

            // Get the value from the factory
            var value = _source.GetRawValue(valueName, property);
            if (value == null)
            {
                // Throw an exception informing the user of the missing value
                throw new MissingValueException(valueName);
            }

            // Validate the value
            _validator.Validate<T>(value, valueName, property);

            // Coerce the value
            var result = _coercer.CoerceTo<T>(value, valueName, property);
            if (result == null)
            {
                // Throw an exception informing the user of the failed coercion
                throw new CoercionException(value, valueName, typeof(T), property);
            }

            // Return the result
            return (T)result;
        }
    }
}
