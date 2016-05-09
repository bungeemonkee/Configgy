using System;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SByteValidatorTests : NumericishValidatorTests<SByteValidatorAttribute, sbyte>
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
                return "128";
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
                return "-129";
            }
        }

        protected override sbyte TestingMax
        {
            get
            {
                return 20;
            }
        }

        protected override sbyte TestingMin
        {
            get
            {
                return 10;
            }
        }

        protected override sbyte TypeMax
        {
            get
            {
                return sbyte.MaxValue;
            }
        }

        protected override sbyte TypeMin
        {
            get
            {
                return sbyte.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override sbyte[] ValidValues
        {
            get
            {
                return new sbyte[] { 3, 6 };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "15";
            }
        }

        protected override INumericishValidator<sbyte> MakeValidator(sbyte min, sbyte max, sbyte[] validValues)
        {
            return validValues == null
                ? new SByteValidatorAttribute(min, max)
                : new SByteValidatorAttribute(validValues);
        }
    }
}
