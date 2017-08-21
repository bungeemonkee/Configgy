using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FloatValidatorTests : NumericishValidatorTests<FloatValidatorAttribute, float>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "3.40282347E+39";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-3.402824E38";

        protected override float TestingMax => 20;

        protected override float TestingMin => 10;

        protected override float TypeMax => float.MaxValue;

        protected override float TypeMin => float.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override float[] ValidValues => new float[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<float> MakeValidator(float min, float max, float[] validValues)
        {
            return validValues == null
                ? new FloatValidatorAttribute(min, max)
                : new FloatValidatorAttribute(validValues);
        }
    }
}