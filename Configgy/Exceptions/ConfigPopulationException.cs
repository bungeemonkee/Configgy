using System;
using System.Collections.Generic;

namespace Configgy.Exceptions
{
    /// <summary>
    /// Exception thrown when any object could not be successfully populated by <see cref="ConfigExtensions.Populate"/>.
    /// </summary>
    public class ConfigPopulationException : AggregateException
    {
        /// <summary>
        /// The exceptions thrown when populating each property, keyed by property name.
        /// </summary>
        public readonly IReadOnlyDictionary<string, Exception> ExceptionsByPropertyName;

        /// <summary>
        /// Create a new ConfigPopulationException with the given set of exceptions keyed by property name.
        /// </summary>
        /// <param name="innerExceptions">The one or more exceptions that caused this exception.</param>
        public ConfigPopulationException(IReadOnlyDictionary<string, Exception> innerExceptions)
            : base(GenerateMessage(innerExceptions.Keys), innerExceptions.Values)
        {
            ExceptionsByPropertyName = innerExceptions;
        }

        private static string GenerateMessage (IEnumerable<string> properties)
        {
            const string format = "Exceptions were encountered when populating the following properties: {0}. Examine the ExceptionsByPropertyName value for more details.";

            var propertiesJoined = string.Join(", ", properties);
            return string.Format(format, propertiesJoined);
        }
    }
}
