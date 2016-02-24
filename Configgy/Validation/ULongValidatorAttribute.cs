using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class ULongValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<ulong>
    {
        public ulong Min { get; protected set; }

        public ulong Max { get; protected set; }

        public ulong[] ValidValues { get; protected set; }

        public ULongValidatorAttribute()
        {
            Min = ulong.MinValue;
            Max = ulong.MaxValue;
        }

        public ULongValidatorAttribute(ulong min, ulong max)
        {
            Min = min;
            Max = max;
        }

        public ULongValidatorAttribute(params ulong[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override object Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = ulong.Parse(value);

            if (val < Min || val > Max)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (ValidValues != null && !ValidValues.Contains(val))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return val;
        }
    }
}
