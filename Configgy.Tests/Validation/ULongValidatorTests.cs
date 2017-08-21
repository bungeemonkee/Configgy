using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ULongValidatorTests : NumericishValidatorTests<ULongValidatorAttribute, ulong>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "18,446,744,073,709,551,616";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-1";

        protected override ulong TestingMax => 20;

        protected override ulong TestingMin => 10;

        protected override ulong TypeMax => ulong.MaxValue;

        protected override ulong TypeMin => ulong.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override ulong[] ValidValues => new ulong[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<ulong> MakeValidator(ulong min, ulong max, ulong[] validValues)
        {
            return validValues == null
                ? new ULongValidatorAttribute(min, max)
                : new ULongValidatorAttribute(validValues);
        }
    }
}