using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CharValidatorTests : NumericishValidatorTests<CharValidatorAttribute, char>
    {
        protected override string AboveTestingMax => "F";

        protected override string AboveTypeMax => "65536";

        protected override string AnInvalidValue => "E";

        protected override string AValidValue => "q";

        protected override string BelowTestingMin => "A";

        protected override string BelowTypeMin => "-1";

        protected override char TestingMax => 'E';

        protected override char TestingMin => 'B';

        protected override char TypeMax => char.MaxValue;

        protected override char TypeMin => char.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override char[] ValidValues => new[] {'Q', 'q', '*'};

        protected override string WithinTestingRange => "C";

        protected override INumericishValidator<char> MakeValidator(char min, char max, char[]? validValues)
        {
            return validValues == null
                ? new CharValidatorAttribute(min, max)
                : new CharValidatorAttribute(validValues);
        }
    }
}