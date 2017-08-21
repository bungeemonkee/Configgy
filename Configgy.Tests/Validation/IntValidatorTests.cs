using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class IntValidatorTests : NumericishValidatorTests<IntValidatorAttribute, int>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "2,147,483,648";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-2,147,483,649";

        protected override int TestingMax => 20;

        protected override int TestingMin => 10;

        protected override int TypeMax => int.MaxValue;

        protected override int TypeMin => int.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override int[] ValidValues => new[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<int> MakeValidator(int min, int max, int[] validValues)
        {
            return validValues == null
                ? new IntValidatorAttribute(min, max)
                : new IntValidatorAttribute(validValues);
        }
    }
}