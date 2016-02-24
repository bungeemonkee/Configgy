using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class IntValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<int>
    {
        public int Min { get; protected set; }

        public int Max { get; protected set; }

        public int[] ValidValues { get; protected set; }

        public IntValidatorAttribute()
        {
            Min = int.MinValue;
            Max = int.MaxValue;
        }

        public IntValidatorAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public IntValidatorAttribute(params int[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override object Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = int.Parse(value);

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
