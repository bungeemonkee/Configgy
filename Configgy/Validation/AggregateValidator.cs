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

        public AggregateValidator()
            : this(GetDefaultValidatorsByType())
        {
        }

        public AggregateValidator(IDictionary<Type, IValueValidator> validatorsByType)
        {
            // Add type-based validators
            _validatorsByType = validatorsByType;
        }

        public void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            // Get the validator for the expected type
            IValueValidator typeValidator;
            if (_validatorsByType.TryGetValue(typeof(T), out typeValidator))
            {
                // Validate based strictly on the type
                typeValidator.Validate<T>(value, valueName, property);
            }

            // If there is no property then return
            if (property == null) return;

            // Get any validators from the property attributes
            var propertyValidators = property
                .GetCustomAttributes(true)
                .OfType<IValueValidator>();

            // Use each property attribute validator to validate the value
            foreach (var validator in propertyValidators)
            {
                validator.Validate<T>(value, valueName, property);
            }
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
