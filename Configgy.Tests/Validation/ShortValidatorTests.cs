using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class ShortValidatorTests : NumericishValidatorTests<ShortValidatorAttribute, short>
    {
        protected override string AboveTestingMax
        {
            get
            {
                return "21";
            }
        }

        protected override string AboveTypeMax
        {
            get
            {
                return "32,768";
            }
        }

        protected override string AnInvalidValue
        {
            get
            {
                return "5";
            }
        }

        protected override string AValidValue
        {
            get
            {
                return "3";
            }
        }

        protected override string BelowTestingMin
        {
            get
            {
                return "9";
            }
        }

        protected override string BelowTypeMin
        {
            get
            {
                return "-32,769";
            }
        }

        protected override short TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override short TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override short TypeMax
        {
            get
            {
                return short.MaxValue;
            }
        }

        protected override short TypeMin
        {
            get
            {
                return short.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override short[] ValidValues
        {
            get
            {
                return new short[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<short> MakeValidator(short min, short max, short[] validValues)
        {
            return validValues == null
                ? new ShortValidatorAttribute(min, max)
                : new ShortValidatorAttribute(validValues);
        }
    }
}
