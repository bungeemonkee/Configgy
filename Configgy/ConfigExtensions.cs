using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Configgy.Exceptions;

namespace Configgy
{
    /// <summary>
    /// Extensions for <see cref="Config"/> instances.
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Validates every configuration property. 
        /// </summary>
        /// <remarks>
        /// Works by dynamically invoking every public readable property on this instance.
        /// This will aggregate and re-throw any exceptions encountered within a single exception derived from AggregateException.
        /// If this completed successfully all configuration values will also be cached as a result.
        /// </remarks>
        /// <exception cref="ConfigValidationException">Thrown when there are one or more validation errors with properties.</exception>
        public static void Validate(this Config config)
        {
            // get all the public readable properties on this configuration type
            var properties = config
                .GetType()
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .OfType<PropertyInfo>()
                .Where(p => p.CanRead)
                .ToList();

            // if there are no properties to verify then return
            if (properties.Count == 0) return;

            var exceptions = new Dictionary<string, Exception>();

            // go through each property
            foreach (var prop in properties)
            {
                try
                {
                    // invoke the property
                    prop.GetValue(config);
                }
                catch (Exception e)
                {
                    // catch any exceptions
                    exceptions.Add(prop.Name, e);
                }
            }

            // if there were exceptions...
            if (exceptions.Count > 0)
            {
                // re-throw the exceptions
                throw new ConfigValidationException(exceptions);
            }
        }
    }
}
