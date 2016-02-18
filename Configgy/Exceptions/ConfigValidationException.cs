using System;
using System.Collections.Generic;

namespace Configgy.Exceptions
{
    public class ConfigValidationException : AggregateException
    {
        public readonly IReadOnlyDictionary<string, Exception> ExceptionsByPropertyName;

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
