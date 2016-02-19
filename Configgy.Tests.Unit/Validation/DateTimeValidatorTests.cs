using System;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    public class DateTimeValidatorTests : NumericishValidatorTests<DateTimeValidatorAttribute, DateTime>
    {
        protected override string AboveTestingMax
        {
            get
            {
                return "2017-01-01";
            }
        }

        protected override string AboveTypeMax
        {
            get
            {
                return "99999-01-01";
            }
        }

        protected override string AnInvalidValue
        {
            get
            {
                return "2015-01-01";
            }
        }

        protected override string AValidValue
        {
            get
            {
                return "2015-01-02";
            }
        }

        protected override string BelowTestingMin
        {
            get
            {
                return "2000-01-01";
            }
        }

        protected override string BelowTypeMin
        {
            get
            {
                return "0000-01-01";
            }
        }

        protected override DateTime TestingMax
        {
            get
            {
                return DateTime.Parse("2016-12-30");
            }
        }

        protected override DateTime TestingMin
        {
            get
            {
                return DateTime.Parse("2016-01-01");
            }
        }

        protected override DateTime TypeMax
        {
            get
            {
                return DateTime.MaxValue;
            }
        }

        protected override DateTime TypeMin
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override DateTime[] ValidValues
        {
            get
            {
                return new DateTime[] { DateTime.Parse("2015-01-02"), DateTime.Parse("2015-02-01") };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "2016-03-01";
            }
        }

        protected override INumericishValidator<DateTime> MakeValidator(DateTime min, DateTime max, DateTime[] validValues)
        {
            return validValues == null
                ? new DateTimeValidatorAttribute(min.ToString(), max.ToString())
                : new DateTimeValidatorAttribute(validValues.Select(d => d.ToString()).ToArray());
        }
    }
}
