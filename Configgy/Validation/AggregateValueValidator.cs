using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    /// <summary>
    /// A validator that actually aggregates other validators by type and from property attributes.
    /// </summary>
    public class AggregateValueValidator : IValueValidator
    {
        private readonly IDictionary<Type, IValueValidator> _validatorsByType;

        public AggregateValueValidator()
        {
            // TODO: Add type-based validators
            _validatorsByType = new Dictionary<Type, IValueValidator>();
        }

        public void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            // Get the validator for the expected type
            IValueValidator typeValidator;
            if (_validatorsByType.TryGetValue(property.PropertyType, out typeValidator))
            {
                // Validate based strictly on the type
                typeValidator.Validate<T>(value, valueName, property);
            }

            // If there is no property then return
            if (property == null) return;

            // Get any validators from the property attributes
            var propertyValidators = property.CustomAttributes
                .OfType<IValueValidator>();

            // Use each property attribute validator to validate the value
            foreach (var validator in propertyValidators)
            {
                validator.Validate<T>(value, valueName, property);
            }
        }
    }
}
