using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    public class ULongValidatorTests : NumericishValidatorTests<ULongValidatorAttribute, ulong>
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
                return "18,446,744,073,709,551,616";
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

        protected override ulong TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override ulong TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override ulong TypeMax
        {
            get
            {
                return ulong.MaxValue;
            }
        }

        protected override ulong TypeMin
        {
            get
            {
                return ulong.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override ulong[] ValidValues
        {
            get
            {
                return new ulong[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<ulong> MakeValidator(ulong min, ulong max, ulong[] validValues)
        {
            return validValues == null
                ? new ULongValidatorAttribute(min, max)
                : new ULongValidatorAttribute(validValues);
        }
    }
}
