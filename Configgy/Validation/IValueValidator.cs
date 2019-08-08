﻿namespace Configgy.Validation
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
        /// <param name="property">The <see cref="IConfigProperty"/> for this value.</param>
        /// <param name="result">If the validator did the coercion it should set this to the result.</param>
        /// <returns>
        ///     True if the validator performed coercion as a side effect, false otherwise.
        ///     Any return value (true or false) indicates successful validation.
        ///     If the validator did not successfully validate the value it should throw an exception, preferably <see cref="Exceptions.ValidationException"/>.
        /// </returns>
        /// <exception cref="Exceptions.ValidationException">Thrown when the value is not valid.</exception>
        bool Validate<T>(IConfigProperty property, string value, out T result);
    }
}
