using System;
using System.Reflection;

namespace Configgy.Exceptions
{
    /// <summary>
    /// Exception to be thrown when any value fails validation.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// The raw string value that failed validation.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// The name of the value that failed validation.
        /// </summary>
        public readonly string ValueName;

        /// <summary>
        /// The property associated with the value that failed validation, or null if there is none.
        /// </summary>
        public readonly ICustomAttributeProvider? Property;

        /// <summary>
        /// Creates a new ValidationException with no inner exception.
        /// </summary>
        /// <param name="value">The raw string value.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="property">The property reference associated with the value.</param>
        public ValidationException(string value, string valueName, ICustomAttributeProvider? property)
            : base(GetMessage(value, valueName))
        {
            Value = value;
            ValueName = valueName;
            Property = property;
        }

        /// <summary>
        /// Creates a new ValidationException with an inner exception.
        /// </summary>
        /// <param name="value">The raw string value.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="property">The property reference associated with the value.</param>
        /// <param name="innerException">The exception that caused the validation to fail.</param>
        public ValidationException(string value, string valueName, ICustomAttributeProvider? property, Exception innerException)
            : base(GetMessage(value, valueName), innerException)
        {
            Value = value;
            ValueName = valueName;
            Property = property;
        }

        private static string GetMessage(string value, string valueName)
        {
            const string format = "Property '{0}' has an invalid value: {1}";
            return string.Format(format, valueName, value);
        }
    }
}
