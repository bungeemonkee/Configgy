using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class LongValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<long>
    {
        public long Min { get; protected set; }

        public long Max { get; protected set; }

        public long[] ValidValues { get; protected set; }

        public LongValidatorAttribute()
        {
            Min = long.MinValue;
            Max = long.MaxValue;
        }

        public LongValidatorAttribute(long min, long max)
        {
            Min = min;
            Max = max;
        }

        public LongValidatorAttribute(params long[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = long.Parse(value);

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
