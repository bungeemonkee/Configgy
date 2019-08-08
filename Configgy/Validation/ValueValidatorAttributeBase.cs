using System;

namespace Configgy.Validation
{
    /// <summary>
    /// Base class for any validator attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class ValueValidatorAttributeBase : Attribute, IValueValidator
    {
        /// <inheritdoc cref="IValueValidator.Validate{T}"/>
        public abstract bool Validate<T>(IConfigProperty property, string value, out T result);
    }
}
