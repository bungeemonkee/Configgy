using System;

namespace Configgy.Exceptions
{
    public class MissingValueException : Exception
    {
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
