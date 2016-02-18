using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class UShortValidatorAttribute : ValueValidatorAtributeBase
    {
        public ushort Min { get; protected set; }

        public ushort Max { get; protected set; }

        public ushort[] ValidValues { get; protected set; }

        public UShortValidatorAttribute()
        {
            Min = ushort.MinValue;
            Max = ushort.MaxValue;
        }

        public UShortValidatorAttribute(ushort min, ushort max)
        {
            Min = min;
            Max = max;
        }

        public UShortValidatorAttribute(params ushort[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = ushort.Parse(value);

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
