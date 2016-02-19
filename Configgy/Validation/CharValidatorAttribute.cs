using System;
using System.Linq;
using System.Reflection;

namespace Configgy.Validation
{
    public class CharValidatorAttribute : ValueValidatorAtributeBase, INumericishValidator<char>
    {
        public char Min { get; protected set; }

        public char Max { get; protected set; }

        public char[] ValidValues { get; protected set; }

        public CharValidatorAttribute()
        {
            Min = char.MinValue;
            Max = char.MaxValue;
        }

        public CharValidatorAttribute(char min, char max)
        {
            Min = min;
            Max = max;
        }

        public CharValidatorAttribute(params char[] validValues)
            : this()
        {
            ValidValues = validValues;
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            var val = char.Parse(value);

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
