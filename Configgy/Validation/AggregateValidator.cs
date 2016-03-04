using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    /// <summary>
    /// A validator that actually aggregates other validators by type and from property attributes.
    /// </summary>
    public class AggregateValidator : IValueValidator
    {
        private readonly IDictionary<Type, IValueValidator> _validatorsByType;

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
        public object Validate<T>(string value, string valueName, PropertyInfo property)
        {
            // Get the validator for the expected type
            // ...validate based on the type
            // ...and get the result
            IValueValidator typeValidator;
            var result = _validatorsByType.TryGetValue(typeof(T), out typeValidator)
                ? typeValidator.Validate<T>(value, valueName, property)
                : null;

            // If there is no property then return
            if (property == null) return result;

            // Get any validators from the property attributes
            var propertyValidators = property
                .GetCustomAttributes(true)
                .OfType<IValueValidator>();

            // Use each property attribute validator to validate the value
            foreach (var validator in propertyValidators)
            {
                var localResult = validator.Validate<T>(value, valueName, property);
                if (result == null)
                {
                    result = localResult;
                }
            }

            return result;
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
