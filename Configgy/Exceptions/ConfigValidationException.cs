using System;
using System.Collections.Generic;

namespace Configgy.Exceptions
{
    /// <summary>
    /// Exception thrown when a <see cref="Config"/> object could not be successfully validated by <see cref="ConfigExtensions.Validate(Config)"/>.
    /// </summary>
    public class ConfigValidationException : AggregateException
    {
        /// <summary>
        /// The exceptions thrown when validating each property, keyed by property name.
        /// </summary>
        public readonly IReadOnlyDictionary<string, Exception> ExceptionsByPropertyName;

        /// <summary>
        /// Create a new ConfigValidationException with the given set of exceptions keyed by property name.
        /// </summary>
        /// <param name="innerExceptions"></param>
        public ConfigValidationException(IReadOnlyDictionary<string, Exception> innerExceptions)
            : base(GenerateMessage(innerExceptions.Keys), innerExceptions.Values)
        {
            ExceptionsByPropertyName = innerExceptions;
        }

        private static string GenerateMessage (IEnumerable<string> properties)
        {
            const string format = "Exceptions were encountered when accessing the following properties: {0}. Examine the ExceptionsByPropertyName value for more details.";

            var propertiesJoined = string.Join(", ", properties);
            return string.Format(format, propertiesJoined);
        }
    }
}
