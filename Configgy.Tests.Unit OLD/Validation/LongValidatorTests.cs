using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LongValidatorTests : NumericishValidatorTests<LongValidatorAttribute, long>
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
                return "9,223,372,036,854,775,808";
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
                return "-9,223,372,036,854,775,809";
            }
        }

        protected override long TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override long TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override long TypeMax
        {
            get
            {
                return long.MaxValue;
            }
        }

        protected override long TypeMin
        {
            get
            {
                return long.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override long[] ValidValues
        {
            get
            {
                return new long[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<long> MakeValidator(long min, long max, long[] validValues)
        {
            return validValues == null
                ? new LongValidatorAttribute(min, max)
                : new LongValidatorAttribute(validValues);
        }
    }
}
