using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Configgy.Validation
{
    /// <summary>
    /// An <see cref="IValueValidator"/> that matches the raw value against a <see cref="Regex"/>.
    /// </summary>
    public class RegexValidatorAttribute : ValueValidatorAtributeBase
    {
        /// <summary>
        /// The expression to validate the raw value against.
        /// </summary>
        public Regex Expression { get; protected set; }

        /// <summary>
        /// Creates a new RegexValidator with the given expression.
        /// </summary>
        /// <param name="expression"></param>
        public RegexValidatorAttribute(string expression)
        {
            Expression = new Regex(expression);
        }

        /// <summary>
        /// Validate a potential value.
        /// This method must throw an exception if the value is invalid.
        /// </summary>
        /// <typeparam name="T">The type the value is expected to be coerced into.</typeparam>
        /// <param name="value">The raw string value.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="property">If this value is directly associated with a property on a <see cref="Config"/> instance this is the reference to that property.</param>
        /// <returns>
        ///     If the validator also coerced the value in the process of validation it may return that value upon successful validation.
        ///     If the validator did not coerce the value but did validate successfully it should return null.
        ///     If the validator did not successfully validate the value it should throw an exception, preferably <see cref="Exceptions.ValidationException"/>.
        /// </returns>
        /// <exception cref="Exceptions.ValidationException">Thrown when the value is not valid.</exception>
        public override object Validate<T>(string value, string valueName, PropertyInfo property)
        {
            if (!Expression.IsMatch(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return null;
        }
    }
}
