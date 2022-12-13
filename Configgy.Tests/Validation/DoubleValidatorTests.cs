using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DoubleValidatorTests : NumericishValidatorTests<DoubleValidatorAttribute, double>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "1.7976931348623157E+309";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-1.7976931348623159E+308";

        protected override double TestingMax => 20;

        protected override double TestingMin => 10;

        protected override double TypeMax => double.MaxValue;

        protected override string TypeMaxString => double.MaxValue.ToString("R");

        protected override double TypeMin => double.MinValue;

        protected override string TypeMinString => double.MinValue.ToString("R");

        protected override string UnParseable => "this little piggy";

        protected override double[] ValidValues => new double[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<double> MakeValidator(double min, double max, double[]? validValues)
        {
            return validValues == null
                ? new DoubleValidatorAttribute(min, max)
                : new DoubleValidatorAttribute(validValues);
        }
    }
}