using System;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    public class ByteValidatorTests : NumericishValidatorTests<ByteValidatorAttribute, byte>
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
                return "256";
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

        protected override byte TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override byte TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override byte TypeMax
        {
            get
            {
                return byte.MaxValue;
            }
        }

        protected override byte TypeMin
        {
            get
            {
                return byte.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override byte[] ValidValues
        {
            get
            {
                return new byte[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<byte> MakeValidator(byte min, byte max, byte[] validValues)
        {
            return validValues == null
                ? new ByteValidatorAttribute(min, max)
                : new ByteValidatorAttribute(validValues);
        }
    }
}
