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
            
            IConfigProperty property = new ConfigProperty(null, typeof(string), null, null);

            var transformer = new AbsolutePathTransformerAttribute();

            var result = transformer.Transform(property, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transform_Returns_Full_Path()
        {
            const string value = "test.text";
            var expected = Path.GetFullPath(value);
            
            IConfigProperty property = new ConfigProperty(null, typeof(string), null, null);

            var transformer = new AbsolutePathTransformerAttribute();

            var result = transformer.Transform(property, value);

            Assert.AreEqual(expected, result);
        }
    }
}