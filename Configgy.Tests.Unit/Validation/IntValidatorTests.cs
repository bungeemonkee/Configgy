using System;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    public class IntValidatorTests : NumericishValidatorTests<IntValidatorAttribute, int>
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
                return "2,147,483,648";
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
                return "-2,147,483,649";
            }
        }

        protected override int TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override int TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override int TypeMax
        {
            get
            {
                return int.MaxValue;
            }
        }

        protected override int TypeMin
        {
            get
            {
                return int.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override int[] ValidValues
        {
            get
            {
                return new int[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<int> MakeValidator(int min, int max, int[] validValues)
        {
            return validValues == null
                ? new IntValidatorAttribute(min, max)
                : new IntValidatorAttribute(validValues);
        }
    }
}
