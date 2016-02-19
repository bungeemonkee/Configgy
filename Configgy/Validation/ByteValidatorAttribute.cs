using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class ByteValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<byte>
    {
        public byte Min { get; protected set; }

        public byte Max { get; protected set; }

        public byte[] ValidValues { get; protected set; }

        public ByteValidatorAttribute()
        {
            Min = byte.MinValue;
            Max = byte.MaxValue;
        }

        public ByteValidatorAttribute(byte min, byte max)
        {
            Min = min;
            Max = max;
        }

        public ByteValidatorAttribute(params byte[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = byte.Parse(value);

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
