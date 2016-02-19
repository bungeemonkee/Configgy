using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class DateTimeValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<DateTime>
    {
        public DateTime Min { get; protected set; }

        public DateTime Max { get; protected set; }

        public DateTime[] ValidValues { get; protected set; }

        public DateTimeValidatorAttribute()
        {
            Min = DateTime.MinValue;
            Max = DateTime.MaxValue;
        }

        public DateTimeValidatorAttribute(string min, string max)
        {
            Min = DateTime.Parse(min);
            Max = DateTime.Parse(max);
        }

        public DateTimeValidatorAttribute(params string[] validValues)
            : this()
        {
            ValidValues = validValues.Select(DateTime.Parse).ToArray();
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = DateTime.Parse(value);

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
