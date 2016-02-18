using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Configgy.Validation
{
    public class RegexValidator : ValueValidatorAtributeBase
    {
        public Regex Expression { get; protected set; }

        public RegexValidator(string expression)
        {
            Expression = new Regex(expression);
        }

        public override void Validate<T>(string value, string valueName, PropertyInfo property)
        {
            if (!Expression.IsMatch(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}
