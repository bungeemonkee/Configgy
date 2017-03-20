using System;
using System.Reflection;

namespace Configgy.Exceptions
{
    /// <summary>
    /// Exception thrown by <see cref="Config"/> when a value can not be coerced into the expected type.
    /// </summary>
    public class CoercionException : Exception
    {
        /// <summary>
        /// The raw string value that could not be coerced.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// The name of the value that could not be coerced.
        /// </summary>
        public readonly string ValueName;

        /// <summary>
        /// The type the value could not be coerced into.
        /// </summary>
        public readonly Type ExpectedType;

        /// <summary>
        /// If there is a property reference associated with this value name this will hold that reference, null otherwise.
        /// </summary>
        public readonly ICustomAttributeProvider Property;

        /// <summary>
        /// Create a new CoercionException without an inner exception.
        /// </summary>
        /// <param name="value">The raw string value.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="expectedType">The type the value could not be coerced into.</param>
        /// <param name="property">The property reference associated with the value, or null if the is none.</param>
        public CoercionException(string value, string valueName, Type expectedType, ICustomAttributeProvider property)
            : base(GetMessage(value, valueName, expectedType))
        {
        }

        /// <summary>
        /// Create a new CoercionException with an inner exception.
        /// </summary>
        /// <param name="value">The raw string value.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="expectedType">The type the value could not be coerced into.</param>
        /// <param name="property">The property reference associated with the value, or null if the is none.</param>
        /// <param name="innerException">The exception that caused the coercion failure.</param>
        public CoercionException(string value, string valueName, Type expectedType, ICustomAttributeProvider property, Exception innerException)
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
