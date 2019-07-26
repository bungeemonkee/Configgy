using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Transformation;

namespace Configgy.Tests.Transformation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LowercaseTransformerAttributeTests
    {
        [TestMethod]
        public void Transform_Returns_Null_For_Null_Values()
        {
            const string value = null;

            var transformer = new LowercaseTransformerAttribute();

            var result = transformer.Transform(value, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transform_Returns_Lowercase()
        {
            const string value = "BLAH BLAH";
            const string expected = "blah blah";

            var transformer = new LowercaseTransformerAttribute();

            var result = transformer.Transform(value, null, null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Transform_Returns_Lowercase_With_Explicit_Culture()
        {
            const string value = "BLAH BLAH";
            const string expected = "blah blah";
            var culture = CultureInfo.InvariantCulture;

            var transformer = new LowercaseTransformerAttribute
            {
                Culture = culture
            };

            var result = transformer.Transform(value, null, null);

            Assert.AreEqual(expected, result);
        }
    }
}