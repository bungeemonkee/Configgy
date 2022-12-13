using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TimeSpanValidatorTests : NumericishValidatorTests<TimeSpanValidatorAttribute, TimeSpan>
    {
        protected override string AboveTestingMax => "00:20:00";

        protected override string AboveTypeMax => "10675199.02:48:06";

        protected override string AnInvalidValue => "00:05:00";

        protected override string AValidValue => "00:32:00";

        protected override string BelowTestingMin => "00:01:00";

        protected override string BelowTypeMin => "-10675199.02:48:06";

        protected override TimeSpan TestingMax => TimeSpan.FromMinutes(8);

        protected override TimeSpan TestingMin => TimeSpan.FromMinutes(6);

        protected override TimeSpan TypeMax => TimeSpan.MaxValue;

        protected override TimeSpan TypeMin => TimeSpan.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override TimeSpan[] ValidValues => new[] {TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(32)};

        protected override string WithinTestingRange => "00:07:00";

        protected override INumericishValidator<TimeSpan> MakeValidator(TimeSpan min, TimeSpan max,
            TimeSpan[]? validValues)
        {
            return validValues == null
                ? new TimeSpanValidatorAttribute(min.ToString(), max.ToString())
                : new TimeSpanValidatorAttribute(validValues.Select(t => t.ToString()).ToArray());
        }
    }
}