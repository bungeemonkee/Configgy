using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class FloatValidatorAttribute : ValueValidatorAtributeBase
    {
        public float Min { get; protected set; }

        public float Max { get; protected set; }

        public float[] ValidValues { get; protected set; }

        public FloatValidatorAttribute()
        {
            Min = float.MinValue;
            Max = float.MaxValue;
        }

        public FloatValidatorAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public FloatValidatorAttribute(params float[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = float.Parse(value);

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
