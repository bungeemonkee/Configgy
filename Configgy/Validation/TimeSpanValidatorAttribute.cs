using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class TimeSpanValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<TimeSpan>
    {
        public TimeSpan Min { get; protected set; }

        public TimeSpan Max { get; protected set; }

        public TimeSpan[] ValidValues { get; protected set; }

        public TimeSpanValidatorAttribute()
        {
            Min = TimeSpan.MinValue;
            Max = TimeSpan.MaxValue;
        }

        public TimeSpanValidatorAttribute(string min, string max)
        {
            Min = TimeSpan.Parse(min);
            Max = TimeSpan.Parse(max);
        }

        public TimeSpanValidatorAttribute(params string[] validValues)
            : this()
        {
            ValidValues = validValues.Select(TimeSpan.Parse).ToArray();
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = TimeSpan.Parse(value);

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
