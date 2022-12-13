using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Coercion;
using Moq;

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

            var propertyMock = new Mock<IConfigProperty>(MockBehavior.Strict);

            var coerced = coercer.Coerce(propertyMock.Object, value, out Regex result);

            Assert.IsNotNull(result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        [ExpectedException(typeof(RegexParseException))]
        public void Coerce_Throws_Exception_For_Invalid_Expression_Strings()
        {
            const string value = "(";

            var coercer = new RegexCoercerAttribute();

            var propertyMock = new Mock<IConfigProperty>(MockBehavior.Strict);

            coercer.Coerce(propertyMock.Object, value, out Regex result);
        }

        [TestMethod]
        public void Coerce_Returns_Null_For_Types_Other_Than_Regex()
        {
            const int expected = default;
            const string value = ".*";

            var coercer = new RegexCoercerAttribute();

            var propertyMock = new Mock<IConfigProperty>(MockBehavior.Strict);

            var coerced = coercer.Coerce(propertyMock.Object, value, out int result);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(coerced);
        }
    }
}