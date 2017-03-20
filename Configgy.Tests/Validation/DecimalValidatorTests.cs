using System;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class DecimalValidatorTests : NumericishValidatorTests<DecimalValidatorAttribute, decimal>
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
                return "79,228,162,514,264,337,593,543,950,336";
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
                return "-79,228,162,514,264,337,593,543,950,336";
            }
        }

        protected override decimal TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override decimal TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override decimal TypeMax
        {
            get
            {
                return decimal.MaxValue;
            }
        }

        protected override decimal TypeMin
        {
            get
            {
                return decimal.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override decimal[] ValidValues
        {
            get
            {
                return new decimal[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<decimal> MakeValidator(decimal min, decimal max, decimal[] validValues)
        {
            return validValues == null
                ? new DecimalValidatorAttribute(min, max)
                : new DecimalValidatorAttribute(validValues);
        }
    }
}
