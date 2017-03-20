using Configgy.Transformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Transformation
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
        public void Transform_Returns_Uppercase()
        {
            const string value = "BLAH BLAH";
            const string expected = "blah blah";

            var transformer = new LowercaseTransformerAttribute();

            var result = transformer.Transform(value, null, null);

            Assert.AreEqual(expected, result);
        }
    }
}
