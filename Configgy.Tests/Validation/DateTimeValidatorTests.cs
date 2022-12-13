using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DateTimeValidatorTests : NumericishValidatorTests<DateTimeValidatorAttribute, DateTime>
    {
        protected override string AboveTestingMax => "2017-01-01";

        protected override string AboveTypeMax => "99999-01-01";

        protected override string AnInvalidValue => "2015-01-01";

        protected override string AValidValue => "2015-01-02";

        protected override string BelowTestingMin => "2000-01-01";

        protected override string BelowTypeMin => "0000-01-01";

        protected override DateTime TestingMax => DateTime.Parse("2016-12-30");

        protected override DateTime TestingMin => DateTime.Parse("2016-01-01");

        protected override DateTime TypeMax => DateTime.MaxValue;

        protected override DateTime TypeMin => DateTime.MinValue;

        protected override string UnParseable => "this little piggy";

        protected override DateTime[] ValidValues => new[] {DateTime.Parse("2015-01-02"), DateTime.Parse("2015-02-01")};

        protected override string WithinTestingRange => "2016-03-01";

        protected override INumericishValidator<DateTime> MakeValidator(DateTime min, DateTime max,
            DateTime[]? validValues)
        {
            return validValues == null
                ? new DateTimeValidatorAttribute(min.ToString("O"), max.ToString("O"))
                : new DateTimeValidatorAttribute(validValues.Select(d => d.ToString()).ToArray());
        }
    }
}