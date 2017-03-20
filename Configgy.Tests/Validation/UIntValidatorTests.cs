using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class UIntValidatorTests : NumericishValidatorTests<UIntValidatorAttribute, uint>
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
                return "4,294,967,296";
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

        protected override uint TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override uint TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override uint TypeMax
        {
            get
            {
                return uint.MaxValue;
            }
        }

        protected override uint TypeMin
        {
            get
            {
                return uint.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override uint[] ValidValues
        {
            get
            {
                return new uint[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<uint> MakeValidator(uint min, uint max, uint[] validValues)
        {
            return validValues == null
                ? new UIntValidatorAttribute(min, max)
                : new UIntValidatorAttribute(validValues);
        }
    }
}
