using System;
using System.Reflection;

namespace Configgy.Exceptions
{
    public class ValidationException : Exception
    {
        public readonly string Value;
        public readonly string ValueName;
        public readonly PropertyInfo Property;

        public ValidationException(string value, string valueName, PropertyInfo property)
            : base(GetMessage(value, valueName))
        {
        }

        public ValidationException(string value, string valueName, PropertyInfo property, Exception innerException)
            : base(GetMessage(value, valueName), innerException)
        {
        }

        private static string GetMessage(string value, string valueName)
        {
            const string format = "Property '{0}' has an invalid value: {1}";
            return string.Format(format, valueName, value);
        }
    }
}
