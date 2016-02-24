using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class SByteValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<sbyte>
    {
        public sbyte Min { get; protected set; }

        public sbyte Max { get; protected set; }

        public sbyte[] ValidValues { get; protected set; }

        public SByteValidatorAttribute()
        {
            Min = sbyte.MinValue;
            Max = sbyte.MaxValue;
        }

        public SByteValidatorAttribute(sbyte min, sbyte max)
        {
            Min = min;
            Max = max;
        }

        public SByteValidatorAttribute(params sbyte[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override object Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = sbyte.Parse(value);

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
