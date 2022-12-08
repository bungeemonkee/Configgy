using System;
using System.Linq;

namespace Configgy.Validation
{
    /// <summary>
    /// An <see cref="IValueValidator"/> for <see cref="sbyte"/>s.
    /// </summary>
    public class SByteValidatorAttribute : ValueValidatorAttributeBase, INumericishValidator<sbyte>
    {
        /// <summary>
        /// The minimum value allowed by this validator.
        /// </summary>
        public sbyte Min { get; protected set; }

        /// <summary>
        /// The maximum value allowed by this validator.
        /// </summary>
        public sbyte Max { get; protected set; }

        /// <summary>
        /// The set of values allowed by this validator.
        /// </summary>
        public sbyte[]? ValidValues { get; protected set; }

        /// <summary>
        /// Creates a validator with default max and min values.
        /// </summary>
        public SByteValidatorAttribute()
        {
            Min = sbyte.MinValue;
            Max = sbyte.MaxValue;
        }

        /// <summary>
        /// Creates a validator with the given max and min values.
        /// </summary>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        public SByteValidatorAttribute(sbyte min, sbyte max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Creates a validator with the given set of valid values.
        /// </summary>
        /// <param name="validValues">The set of values to be considered valid; any other values will cause an exception.</param>
        public SByteValidatorAttribute(params sbyte[] validValues)
            : this()
        {
            Min = sbyte.MinValue;
            Max = sbyte.MaxValue;
            ValidValues = validValues;
        }

        /// <inheritdoc cref="IValueValidator.Validate{T}"/>
        public override bool Validate<T>(IConfigProperty property, string? value, out T result)
        {
            if (value == null)
            {
                result = default!;
                return false;
            }

            var val = sbyte.Parse(value);

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
