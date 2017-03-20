using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class FloatValidatorTests : NumericishValidatorTests<FloatValidatorAttribute, float>
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
                return "3.40282347E+39";
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
                return "-3.402824E38";
            }
        }

        protected override float TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override float TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override float TypeMax
        {
            get
            {
                return float.MaxValue;
            }
        }

        protected override float TypeMin
        {
            get
            {
                return float.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override float[] ValidValues
        {
            get
            {
                return new float[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<float> MakeValidator(float min, float max, float[] validValues)
        {
            return validValues == null
                ? new FloatValidatorAttribute(min, max)
                : new FloatValidatorAttribute(validValues);
        }
    }
}
