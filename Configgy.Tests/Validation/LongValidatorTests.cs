using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LongValidatorTests : NumericishValidatorTests<LongValidatorAttribute, long>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "9,223,372,036,854,775,808";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-9,223,372,036,854,775,809";

        protected override long TestingMax => 20;

        protected override long TestingMin => 10;

        protected override long TypeMax => long.MaxValue;

        protected override long TypeMin => long.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override long[] ValidValues => new long[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<long> MakeValidator(long min, long max, long[] validValues)
        {
            return validValues == null
                ? new LongValidatorAttribute(min, max)
                : new LongValidatorAttribute(validValues);
        }
    }
}