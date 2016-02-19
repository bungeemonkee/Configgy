using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    public class RegexValidatorTests
    {
        [TestMethod]
        public void Validate_Allows_Valid_Value()
        {
            var valdator = new RegexValidator("aaabbbccc");

            valdator.Validate<string>("aaabbbccc", "testing", null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Validate_Throws_Exception_For_Invalid_Value()
        {
            var valdator = new RegexValidator("aaabbbccc");

            valdator.Validate<string>("the number twelve", "testing", null);
        }
    }
}
