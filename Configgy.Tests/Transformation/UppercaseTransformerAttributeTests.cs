using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Transformation;

namespace Configgy.Tests.Transformation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UppercaseTransformerAttributeTests
    {
        [TestMethod]
        public void Transform_Returns_Null_For_Null_Values()
        {
            const string value = null;

            var transformer = new UppercaseTransformerAttribute();

            var result = transformer.Transform(value, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transform_Returns_Uppercase()
        {
            const string value = "blah blah";
            const string expected = "BLAH BLAH";

            var transformer = new UppercaseTransformerAttribute();

            var result = transformer.Transform(value, null, null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Transform_Returns_Lowercase_With_Explicit_Culture()
        {
            const string value = "blah blah";
            const string expected = "BLAH BLAH";
            var culture = CultureInfo.InvariantCulture;

            var transformer = new UppercaseTransformerAttribute
            {
                Culture = culture
            };

            var result = transformer.Transform(value, null, null);

            Assert.AreEqual(expected, result);
        }
    }
}