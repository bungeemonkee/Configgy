using System;
using System.Linq;

namespace Configgy.Validation
{
    /// <summary>
    /// An <see cref="IValueValidator"/> for <see cref="decimal"/>s.
    /// </summary>
    public class DecimalValidatorAttribute : ValueValidatorAttributeBase, INumericishValidator<decimal>
    {
        /// <summary>
        /// The minimum value allowed by this validator.
        /// </summary>
        public decimal Min { get; protected set; }

        /// <summary>
        /// The maximum value allowed by this validator.
        /// </summary>
        public decimal Max { get; protected set; }

        /// <summary>
        /// The set of values allowed by this validator.
        /// </summary>
        public decimal[] ValidValues { get; protected set; }

        /// <summary>
        /// Creates a validator with default max and min values.
        /// </summary>
        public DecimalValidatorAttribute()
        {
            Min = decimal.MinValue;
            Max = decimal.MaxValue;
        }

        /// <summary>
        /// Creates a validator with the given max and min values.
        /// </summary>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        public DecimalValidatorAttribute(decimal min, decimal max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Creates a validator with the given set of valid values.
        /// </summary>
        /// <param name="validValues">The set of values to be considered valid; any other values will cause an exception.</param>
        public DecimalValidatorAttribute(params decimal[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        /// <inheritdoc cref="IValueValidator.Validate{T}"/>
        public override bool Validate<T>(IConfigProperty property, string value, out T result)
        {
            var val = decimal.Parse(value);

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
