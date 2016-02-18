using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class DecimalValidatorAttribute : ValueValidatorAtributeBase
    {
        public decimal Min { get; protected set; }

        public decimal Max { get; protected set; }

        public decimal[] ValidValues { get; protected set; }

        public DecimalValidatorAttribute()
        {
            Min = decimal.MinValue;
            Max = decimal.MaxValue;
        }

        public DecimalValidatorAttribute(decimal min, decimal max)
        {
            Min = min;
            Max = max;
        }

        public DecimalValidatorAttribute(params decimal[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
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
        }
    }
}
