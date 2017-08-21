using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UShortValidatorTests : NumericishValidatorTests<UShortValidatorAttribute, ushort>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "65,535";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-1";

        protected override ushort TestingMax => 20;

        protected override ushort TestingMin => 10;

        protected override ushort TypeMax => ushort.MaxValue;

        protected override ushort TypeMin => ushort.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override ushort[] ValidValues => new ushort[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<ushort> MakeValidator(ushort min, ushort max, ushort[] validValues)
        {
            return validValues == null
                ? new UShortValidatorAttribute(min, max)
                : new UShortValidatorAttribute(validValues);
        }
    }
}