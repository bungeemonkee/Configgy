using System;
using System.Text.RegularExpressions;

namespace Configgy.Validation
{
    /// <summary>
    /// An <see cref="IValueValidator"/> that matches the raw value against a <see cref="Regex"/>.
    /// </summary>
    public class RegexValidatorAttribute : ValueValidatorAttributeBase
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

        /// <inheritdoc cref="IValueValidator.Validate{T}"/>
        public override bool Validate<T>(IConfigProperty property, string? value, out T result)
        {
            if (value != null && !Expression.IsMatch(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            result = default!;
            return false;
        }
    }
}
