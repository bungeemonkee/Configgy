using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class DoubleValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<double>
    {
        public double Min { get; protected set; }

        public double Max { get; protected set; }

        public double[] ValidValues { get; protected set; }

        public DoubleValidatorAttribute()
        {
            Min = double.MinValue;
            Max = double.MaxValue;
        }

        public DoubleValidatorAttribute(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public DoubleValidatorAttribute(params double[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = double.Parse(value);

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
