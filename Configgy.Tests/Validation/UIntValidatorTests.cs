using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UIntValidatorTests : NumericishValidatorTests<UIntValidatorAttribute, uint>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "4,294,967,296";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-1";

        protected override uint TestingMax => 20;

        protected override uint TestingMin => 10;

        protected override uint TypeMax => uint.MaxValue;

        protected override uint TypeMin => uint.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override uint[] ValidValues => new uint[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<uint> MakeValidator(uint min, uint max, uint[]? validValues)
        {
            return validValues == null
                ? new UIntValidatorAttribute(min, max)
                : new UIntValidatorAttribute(validValues);
        }
    }
}