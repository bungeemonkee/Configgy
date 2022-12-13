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
    public class UppercaseTransformerAttributeTests
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

            var transformer = new UppercaseTransformerAttribute();

            var result = transformer.Transform(property, value);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transform_Returns_Uppercase()
        {
            const string value = "blah blah";
            const string expected = "BLAH BLAH";
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty("value", typeof(string), propertyMock.Object, null);

            var transformer = new UppercaseTransformerAttribute();

            var result = transformer.Transform(property, value);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Transform_Returns_Lowercase_With_Explicit_Culture()
        {
            const string value = "blah blah";
            const string expected = "BLAH BLAH";
            var culture = CultureInfo.InvariantCulture;
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty("value", typeof(string), propertyMock.Object, null);

            var transformer = new UppercaseTransformerAttribute
            {
                Culture = culture
            };

            var result = transformer.Transform(property, value);

            Assert.AreEqual(expected, result);
        }
    }
}