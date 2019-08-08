using System;
using System.Collections.Generic;
using System.Linq;

namespace Configgy.Validation
{
    /// <summary>
    /// A validator that actually aggregates other validators by type and from property attributes.
    /// </summary>
    public class AggregateValidator : IValueValidator
    {
        private readonly IDictionary<Type, IValueValidator> _validatorsByType;

        /// <summary>
        /// The <see cref="IValueValidator"/>s used by the type they are used for.
        /// This does not include ones defined as property attributes.
        /// </summary>
        public IReadOnlyDictionary<Type, IValueValidator> ValidatorsByType => _validatorsByType as IReadOnlyDictionary<Type, IValueValidator>;

        /// <summary>
        /// Creates an AggregateValidator with a default set of type-specific validators.
        /// </summary>
        public AggregateValidator()
            : this(GetDefaultValidatorsByType())
        {
        }

        /// <summary>
        /// Creates an AggregateValidator with the given set of type-specific validators.
        /// </summary>
        /// <param name="validatorsByType"></param>
        public AggregateValidator(IDictionary<Type, IValueValidator> validatorsByType)
        {
            // Add type-based validators
            _validatorsByType = validatorsByType;
        }

        /// <inheritdoc cref="IValueValidator.Validate{T}"/>
        public bool Validate<T>(IConfigProperty property, string value, out T result)
        {
            // Set the result to the default value
            result = default;

            // Get the validator for the expected type
            // ...validate based on the type
            // ...and get the result
            var coerced = _validatorsByType.TryGetValue(typeof(T), out var typeValidator) && typeValidator.Validate(property, value, out result);

            // If there is no property then return
            if (property == null) return coerced;

            // Get any validators from the property attributes
            var propertyValidators = property.Attributes
                .OfType<IValueValidator>();

            // Use each property attribute validator to validate the value
            foreach (var validator in propertyValidators)
            {
                if (validator.Validate(property, value, out T localResult) && !coerced)
                {
                    result = localResult;
                    coerced = true;
                }
            }

            return coerced;
        }

        private static IDictionary<Type, IValueValidator> GetDefaultValidatorsByType()
        {
            return new Dictionary<Type, IValueValidator>
            {
                [typeof(byte)] = new ByteValidatorAttribute(),
                [typeof(char)] = new CharValidatorAttribute(),
                [typeof(DateTime)] = new DateTimeValidatorAttribute(),
                [typeof(decimal)] = new DecimalValidatorAttribute(),
                [typeof(double)] = new DoubleValidatorAttribute(),
                [typeof(float)] = new FloatValidatorAttribute(),
                [typeof(int)] = new IntValidatorAttribute(),
                [typeof(long)] = new LongValidatorAttribute(),
                [typeof(sbyte)] = new SByteValidatorAttribute(),
                [typeof(short)] = new ShortValidatorAttribute(),
                [typeof(TimeSpan)] = new TimeSpanValidatorAttribute(),
                [typeof(uint)] = new UIntValidatorAttribute(),
                [typeof(ulong)] = new ULongValidatorAttribute(),
                [typeof(ushort)] = new UShortValidatorAttribute()
            };
        }
    }
}
