using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ByteValidatorTests : NumericishValidatorTests<ByteValidatorAttribute, byte>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "256";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-1";

        protected override byte TestingMax => 20;

        protected override byte TestingMin => 10;

        protected override byte TypeMax => byte.MaxValue;

        protected override byte TypeMin => byte.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override byte[] ValidValues => new byte[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<byte> MakeValidator(byte min, byte max, byte[] validValues)
        {
            return validValues == null
                ? new ByteValidatorAttribute(min, max)
                : new ByteValidatorAttribute(validValues);
        }
    }
}