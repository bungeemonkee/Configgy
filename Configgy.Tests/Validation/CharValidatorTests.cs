using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class CharValidatorTests : NumericishValidatorTests<CharValidatorAttribute, char>
    {
        protected override string AboveTestingMax
        {
            get
            {
                return "F";
            }
        }

        protected override string AboveTypeMax
        {
            get
            {
                return "65536";
            }
        }

        protected override string AnInvalidValue
        {
            get
            {
                return "E";
            }
        }

        protected override string AValidValue
        {
            get
            {
                return "q";
            }
        }

        protected override string BelowTestingMin
        {
            get
            {
                return "A";
            }
        }

        protected override string BelowTypeMin
        {
            get
            {
                return "-1";
            }
        }

        protected override char TestingMax
        {
            get
            {
                return 'E';
            }
        }

        protected override char TestingMin
        {
            get
            {
                return 'B';
            }
        }

        protected override char TypeMax
        {
            get
            {
                return char.MaxValue;
            }
        }

        protected override char TypeMin
        {
            get
            {
                return char.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override char[] ValidValues
        {
            get
            {
                return new char[] { 'Q', 'q', '*' };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "C";
            }
        }

        protected override INumericishValidator<char> MakeValidator(char min, char max, char[] validValues)
        {
            return validValues == null
                ? new CharValidatorAttribute(min, max)
                : new CharValidatorAttribute(validValues);
        }
    }
}
