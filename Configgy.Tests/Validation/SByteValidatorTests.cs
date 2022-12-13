using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SByteValidatorTests : NumericishValidatorTests<SByteValidatorAttribute, sbyte>
    {
        protected override string AboveTestingMax => "21";

        protected override string AboveTypeMax => "128";

        protected override string AnInvalidValue => "5";

        protected override string AValidValue => "3";

        protected override string BelowTestingMin => "9";

        protected override string BelowTypeMin => "-129";

        protected override sbyte TestingMax => 20;

        protected override sbyte TestingMin => 10;

        protected override sbyte TypeMax => sbyte.MaxValue;

        protected override sbyte TypeMin => sbyte.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override sbyte[] ValidValues => new sbyte[] {3, 6};

        protected override string WithinTestingRange => "15";

        protected override INumericishValidator<sbyte> MakeValidator(sbyte min, sbyte max, sbyte[]? validValues)
        {
            return validValues == null
                ? new SByteValidatorAttribute(min, max)
                : new SByteValidatorAttribute(validValues);
        }
    }
}