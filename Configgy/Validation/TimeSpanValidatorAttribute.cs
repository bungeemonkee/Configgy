using System;
using System.Linq;

namespace Configgy.Validation
{
    /// <summary>
    /// An <see cref="IValueValidator"/> for <see cref="TimeSpan"/>s.
    /// </summary>
    public class TimeSpanValidatorAttribute : ValueValidatorAttributeBase, INumericishValidator<TimeSpan>
    {
        /// <summary>
        /// The minimum value allowed by this validator.
        /// </summary>
        public TimeSpan Min { get; protected set; }

        /// <summary>
        /// The maximum value allowed by this validator.
        /// </summary>
        public TimeSpan Max { get; protected set; }

        /// <summary>
        /// The set of values allowed by this validator.
        /// </summary>
        public TimeSpan[] ValidValues { get; protected set; }

        /// <summary>
        /// Creates a validator with default max and min values.
        /// </summary>
        public TimeSpanValidatorAttribute()
        {
            Min = TimeSpan.MinValue;
            Max = TimeSpan.MaxValue;
        }

        /// <summary>
        /// Creates a validator with the given max and min values.
        /// </summary>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        public TimeSpanValidatorAttribute(string min, string max)
        {
            Min = TimeSpan.Parse(min);
            Max = TimeSpan.Parse(max);
        }

        /// <summary>
        /// Creates a validator with the given set of valid values.
        /// </summary>
        /// <param name="validValues">The set of values to be considered valid; any other values will cause an exception.</param>
        public TimeSpanValidatorAttribute(params string[] validValues)
            : this()
        {
            ValidValues = validValues.Select(TimeSpan.Parse).ToArray();
        }

        /// <inheritdoc cref="IValueValidator.Validate{T}"/>
        public override bool Validate<T>(IConfigProperty property, string value, out T result)
        {
            var val = TimeSpan.Parse(value);

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
