using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class RegexCoercerTests
    {
        [TestMethod]
        public void Coerce_Returns_Regex_For_Valid_Expression_Strings()
        {
            const string value = ".*";

            var coercer = new RegexCoercerAttribute();

            Regex result;
            var coerced = coercer.Coerce(value, null, null, out result);

            Assert.IsNotNull(result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Coerce_Throws_Exception_For_Invalid_Expression_Strings()
        {
            const string value = "(";

            var coercer = new RegexCoercerAttribute();

            Regex result;
            coercer.Coerce(value, null, null, out result);
        }

        [TestMethod]
        public void Coerce_Returns_Null_For_Types_Other_Than_Regex()
        {
            const int expected = default(int);
            const string value = ".*";

            var coercer = new RegexCoercerAttribute();

            int result;
            var coerced = coercer.Coerce(value, null, null, out result);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(coerced);
        }
    }
}
