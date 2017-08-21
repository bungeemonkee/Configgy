using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Transformation;

namespace Configgy.Tests.Transformation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AbsolutePathTransformerAttributeTests
    {
        [TestMethod]
        public void Transform_Returns_Null_For_Null_Values()
        {
            const string value = null;

            var transformer = new AbsolutePathTransformerAttribute();

            var result = transformer.Transform(value, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transform_Returns_Full_Path()
        {
            const string value = "test.text";
            var expected = Path.GetFullPath(value);

            var transformer = new AbsolutePathTransformerAttribute();

            var result = transformer.Transform(value, null, null);

            Assert.AreEqual(expected, result);
        }
    }
}