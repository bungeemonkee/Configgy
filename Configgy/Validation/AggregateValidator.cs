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
        /// <param name="result">If the validator did the coercion it should set this to the result.</param>
        /// <returns>
        ///     True if the validator performed coercion as a side effect, false otherwise.
        ///     Any return value (true or false) indicates successful validation.
        ///     If the validator did not successfully validate the value it should throw an exception, preferably <see cref="Exceptions.ValidationException"/>.
        /// </returns>
        /// <exception cref="Exceptions.ValidationException">Thrown when the value is not valid.</exception>
        public bool Validate<T>(string value, string valueName, ICustomAttributeProvider property, out T result)
        {
            // Set the result to the default value
            result = default(T);

            // Get the validator for the expected type
            // ...validate based on the type
            // ...and get the result
            var coerced = _validatorsByType.TryGetValue(typeof(T), out IValueValidator typeValidator) && typeValidator.Validate(value, valueName, property, out result);

            // If there is no property then return
            if (property == null) return coerced;

            // Get any validators from the property attributes
            var propertyValidators = property
                .GetCustomAttributes(true)
                .OfType<IValueValidator>();

            // Use each property attribute validator to validate the value
            foreach (var validator in propertyValidators)
            {
                if (validator.Validate(value, valueName, property, out T localResult) && !coerced)
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
