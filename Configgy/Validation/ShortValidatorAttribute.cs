using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class ShortValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<short>
    {
        public short Min { get; protected set; }

        public short Max { get; protected set; }

        public short[] ValidValues { get; protected set; }

        public ShortValidatorAttribute()
        {
            Min = short.MinValue;
            Max = short.MaxValue;
        }

        public ShortValidatorAttribute(short min, short max)
        {
            Min = min;
            Max = max;
        }

        public ShortValidatorAttribute(params short[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = short.Parse(value);

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
