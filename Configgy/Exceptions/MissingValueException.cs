using System;

namespace Configgy.Exceptions
{
    /// <summary>
    /// Exception thrown when a <see cref="Config"/> instance can not find a source for a given value.
    /// </summary>
    public class MissingValueException : Exception
    {
        /// <summary>
        /// Creates a new MissingValueException for the given value name.
        /// </summary>
        /// <param name="valueName">The name of the value that was not found in any source.</param>
        public MissingValueException(string valueName)
            : base(GetMessage(valueName))
        {
        }

        private static string GetMessage(string valueName)
        {
            const string format = "No source has a configuration value for property '{0}'";

            return string.Format(format, valueName);
        }
    }
}
