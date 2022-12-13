using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Transformation;
using Moq;

namespace Configgy.Tests.Transformation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AbsolutePathTransformerAttributeTests
    {
        [TestMethod]
        public void Transform_Returns_Null_For_Null_Values()
        {
            const string? value = null;
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty("value", typeof(string), propertyMock.Object, null);

            var transformer = new AbsolutePathTransformerAttribute();

            var result = transformer.Transform(property, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transform_Returns_Full_Path()
        {
            const string value = "test.text";
            var expected = Path.GetFullPath(value);
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty("value", typeof(string), propertyMock.Object, null);

            var transformer = new AbsolutePathTransformerAttribute();

            var result = transformer.Transform(property, value);

            Assert.AreEqual(expected, result);
        }
    }
}