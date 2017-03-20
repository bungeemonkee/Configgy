using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class DoubleValidatorTests : NumericishValidatorTests<DoubleValidatorAttribute, double>
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
                return "1.7976931348623157E+309";
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
                return "-1.7976931348623159E+308";
            }
        }

        protected override double TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override double TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override double TypeMax
        {
            get
            {
                return double.MaxValue;
            }
        }

        protected override string TypeMaxString
        {
            get
            {
                return double.MaxValue.ToString("R");
            }
        }

        protected override double TypeMin
        {
            get
            {
                return double.MinValue;
            }
        }

        protected override string TypeMinString
        {
            get
            {
                return double.MinValue.ToString("R");
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override double[] ValidValues
        {
            get
            {
                return new double[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<double> MakeValidator(double min, double max, double[] validValues)
        {
            return validValues == null
                ? new DoubleValidatorAttribute(min, max)
                : new DoubleValidatorAttribute(validValues);
        }
    }
}
