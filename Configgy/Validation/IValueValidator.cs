using System.Reflection;

namespace Configgy.Validation
{
    /// <summary>
    /// Defines a value validator.
    /// </summary>
    public interface IValueValidator
    {
        /// <summary>
        /// Validate a potential value.
        /// This method must throw an exception if the value is invalid.
        /// </summary>
        /// <typeparam name="T">The type the value is expected to be coerced into.</typeparam>
        /// <param name="value">The raw string value.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="property">If this value is directly associated with a property on a <see cref="Config"/> instance this is the reference to that property.</param>
        /// <exception cref="Exceptions.ValidationException">Thrown when the value is not valid.</exception>
        void Validate<T>(string value, string valueName, PropertyInfo property);
    }
}
