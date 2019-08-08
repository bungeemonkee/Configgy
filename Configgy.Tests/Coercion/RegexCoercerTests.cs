using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Coercion;

namespace Configgy.Tests.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegexCoercerTests
    {
        [TestMethod]
        public void Coerce_Returns_Regex_For_Valid_Expression_Strings()
        {
            const string value = ".*";

            var coercer = new RegexCoercerAttribute();

            var coerced = coercer.Coerce(null, value, out Regex result);

            Assert.IsNotNull(result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Coerce_Throws_Exception_For_Invalid_Expression_Strings()
        {
            const string value = "(";

            var coercer = new RegexCoercerAttribute();

            coercer.Coerce(null, value, out Regex result);
        }

        [TestMethod]
        public void Coerce_Returns_Null_For_Types_Other_Than_Regex()
        {
            const int expected = default;
            const string value = ".*";

            var coercer = new RegexCoercerAttribute();

            var coerced = coercer.Coerce(null, value, out int result);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(coerced);
        }
    }
}