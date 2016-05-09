using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegexCoercerTests
    {
        [TestMethod]
        public void CoerceTo_Returns_Regex_For_Valid_Expression_Strings()
        {
            var value = ".*";

            var coercer = new RegexCoercerAttribute();

            var result = coercer.CoerceTo<Regex>(value, null, null);

            Assert.IsInstanceOfType(result, typeof(Regex));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CoerceTo_Throws_Exception_For_Invalid_Expression_Strings()
        {
            var value = "(";

            var coercer = new RegexCoercerAttribute();

            var result = coercer.CoerceTo<Regex>(value, null, null);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_For_Types_Other_Than_Regex()
        {
            var value = ".*";

            var coercer = new RegexCoercerAttribute();

            var result = coercer.CoerceTo<int>(value, null, null);

            Assert.IsNull(result);
        }
    }
}
