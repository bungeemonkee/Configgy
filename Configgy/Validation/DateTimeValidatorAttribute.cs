using System;
using System.Linq;

namespace Configgy.Validation
{
    /// <summary>
    /// An <see cref="IValueValidator"/> for <see cref="DateTime"/>s.
    /// </summary>
    public class DateTimeValidatorAttribute : ValueValidatorAttributeBase, INumericishValidator<DateTime>
    {
        /// <summary>
        /// The minimum value allowed by this validator.
        /// </summary>
        public DateTime Min { get; protected set; }

        /// <summary>
        /// The maximum value allowed by this validator.
        /// </summary>
        public DateTime Max { get; protected set; }

        /// <summary>
        /// The set of values allowed by this validator.
        /// </summary>
        public DateTime[]? ValidValues { get; protected set; }

        /// <summary>
        /// Creates a validator with default max and min values.
        /// </summary>
        public DateTimeValidatorAttribute()
        {
            Min = DateTime.MinValue;
            Max = DateTime.MaxValue;
        }

        /// <summary>
        /// Creates a validator with the given max and min values.
        /// </summary>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        public DateTimeValidatorAttribute(string min, string max)
        {
            Min = DateTime.Parse(min);
            Max = DateTime.Parse(max);
        }

        /// <summary>
        /// Creates a validator with the given set of valid values.
        /// </summary>
        /// <param name="validValues">The set of values to be considered valid; any other values will cause an exception.</param>
        public DateTimeValidatorAttribute(params string[] validValues)
            : this()
        {
            Min = DateTime.MinValue;
            Max = DateTime.MaxValue;
            ValidValues = validValues.Select(DateTime.Parse).ToArray();
        }

        /// <inheritdoc cref="IValueValidator.Validate{T}"/>
        public override bool Validate<T>(IConfigProperty property, string? value, out T result)
        {
            if (value == null)
            {
                result = default!;
                return false;
            }
            
            var val = DateTime.Parse(value);

            if (val < Min || val > Max)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (ValidValues != null && !ValidValues.Contains(val))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            result = (T)(object)val;
            return true;
        }
    }
}
