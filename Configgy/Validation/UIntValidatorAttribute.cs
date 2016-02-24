using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class UIntValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<uint>
    {
        public uint Min { get; protected set; }

        public uint Max { get; protected set; }

        public uint[] ValidValues { get; protected set; }

        public UIntValidatorAttribute()
        {
            Min = uint.MinValue;
            Max = uint.MaxValue;
        }

        public UIntValidatorAttribute(uint min, uint max)
        {
            Min = min;
            Max = max;
        }

        public UIntValidatorAttribute(params uint[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override object Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = uint.Parse(value);

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
