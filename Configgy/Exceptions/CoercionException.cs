using System;
using System.Reflection;

namespace Configgy.Exceptions
{
    public class CoercionException : Exception
    {
        public readonly string Value;
        public readonly string ValueName;
        public readonly Type ExpectedType;
        public readonly PropertyInfo Property;

        public CoercionException(string value, string valueName, Type expectedType, PropertyInfo property)
            : base(GetMessage(value, valueName, expectedType))
        {
        }

        public CoercionException(string value, string valueName, Type expectedType, PropertyInfo property, Exception innerException)
            : base(GetMessage(value, valueName, expectedType), innerException)
        {
        }

        private static string GetMessage(string value, string valueName, Type expectedType)
        {
            const string format = "Property '{0}' can not be coerced into type '{1}'. Value: {2}";
            return string.Format(format, valueName, expectedType.FullName, value);
        }
    }
}
