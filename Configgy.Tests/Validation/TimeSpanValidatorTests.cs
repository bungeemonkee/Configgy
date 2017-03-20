using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class TimeSpanValidatorTests : NumericishValidatorTests<TimeSpanValidatorAttribute, TimeSpan>
    {
        protected override string AboveTestingMax
        {
            get
            {
                return "00:20:00";
            }
        }

        protected override string AboveTypeMax
        {
            get
            {
                return "10675199.02:48:06";
            }
        }

        protected override string AnInvalidValue
        {
            get
            {
                return "00:05:00";
            }
        }

        protected override string AValidValue
        {
            get
            {
                return "00:32:00";
            }
        }

        protected override string BelowTestingMin
        {
            get
            {
                return "00:01:00";
            }
        }

        protected override string BelowTypeMin
        {
            get
            {
                return "-10675199.02:48:06";
            }
        }

        protected override TimeSpan TestingMax
        {
            get
            {
                return TimeSpan.FromMinutes(8);
            }
        }

        protected override TimeSpan TestingMin
        {
            get
            {
                return TimeSpan.FromMinutes(6);
            }
        }

        protected override TimeSpan TypeMax
        {
            get
            {
                return TimeSpan.MaxValue;
            }
        }

        protected override TimeSpan TypeMin
        {
            get
            {
                return TimeSpan.MinValue;
            }
        }

        protected override string UnParseable
        {
            get
            {
                return "this little piggy";
            }
        }

        protected override TimeSpan[] ValidValues
        {
            get
            {
                return new TimeSpan[] { TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(32) };
            }
        }

        protected override string WithinTestingRange
        {
            get
            {
                return "00:07:00";
            }
        }

        protected override INumericishValidator<TimeSpan> MakeValidator(TimeSpan min, TimeSpan max, TimeSpan[] validValues)
        {
            return validValues == null
                ? new TimeSpanValidatorAttribute(min.ToString(), max.ToString())
                : new TimeSpanValidatorAttribute(validValues.Select(t => t.ToString()).ToArray());
        }
    }
}
