using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ShortValidatorTests : NumericishValidatorTests<ShortValidatorAttribute, short>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "32,768";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-32,769";

        protected override short TestingMax => 20;

        protected override short TestingMin => 10;

        protected override short TypeMax => short.MaxValue;

        protected override short TypeMin => short.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override short[] ValidValues => new short[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<short> MakeValidator(short min, short max, short[]? validValues)
        {
            return validValues == null
                ? new ShortValidatorAttribute(min, max)
                : new ShortValidatorAttribute(validValues);
        }
    }
}