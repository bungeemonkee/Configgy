using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    public class UShortValidatorTests : NumericishValidatorTests<UShortValidatorAttribute, ushort>
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
                return "65,535";
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
                return "-1";
            }
        }

        protected override ushort TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override ushort TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override ushort TypeMax
        {
            get
            {
                return ushort.MaxValue;
            }
        }

        protected override ushort TypeMin
        {
            get
            {
                return ushort.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override ushort[] ValidValues
        {
            get
            {
                return new ushort[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<ushort> MakeValidator(ushort min, ushort max, ushort[] validValues)
        {
            return validValues == null
                ? new UShortValidatorAttribute(min, max)
                : new UShortValidatorAttribute(validValues);
        }
    }
}
