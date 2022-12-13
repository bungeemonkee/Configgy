using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Transformation;
using Moq;

namespace Configgy.Tests.Transformation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LowercaseTransformerAttributeTests
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

            var transformer = new LowercaseTransformerAttribute();

            var result = transformer.Transform(property, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transform_Returns_Lowercase()
        {
            const string value = "BLAH BLAH";
            const string expected = "blah blah";
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty("value", typeof(string), propertyMock.Object, null);

            var transformer = new LowercaseTransformerAttribute();

            var result = transformer.Transform(property, value);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Transform_Returns_Lowercase_With_Explicit_Culture()
        {
            const string value = "BLAH BLAH";
            const string expected = "blah blah";
            var culture = CultureInfo.InvariantCulture;
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty("value", typeof(string), propertyMock.Object, null);

            var transformer = new LowercaseTransformerAttribute
            {
                Culture = culture
            };

            var result = transformer.Transform(property, value);

            Assert.AreEqual(expected, result);
        }
    }
}