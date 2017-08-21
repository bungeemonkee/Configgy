using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DecimalValidatorTests : NumericishValidatorTests<DecimalValidatorAttribute, decimal>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "79,228,162,514,264,337,593,543,950,336";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-79,228,162,514,264,337,593,543,950,336";

        protected override decimal TestingMax => 20;

        protected override decimal TestingMin => 10;

        protected override decimal TypeMax => decimal.MaxValue;

        protected override decimal TypeMin => decimal.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override decimal[] ValidValues => new decimal[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<decimal> MakeValidator(decimal min, decimal max, decimal[] validValues)
        {
            return validValues == null
                ? new DecimalValidatorAttribute(min, max)
                : new DecimalValidatorAttribute(validValues);
        }
    }
}