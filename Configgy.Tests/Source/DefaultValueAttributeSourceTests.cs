using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Configgy.Source;

namespace Configgy.Tests.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultValueAttributeValueSourceTests
    {
        [TestMethod]
        public void Get_Returns_Null_When_No_Attributes()
        {
            var source = new DefaultValueAttributeSource();
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty("something", typeof(string), propertyMock.Object, null);

            var result = source.Get(property, out var value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_DefaultValueAttribute()
        {
            const string name = "name";
            const string expected = "1";

            var defaultValueAttributeMock = new Mock<DefaultValueAttribute>(name);
            defaultValueAttributeMock.SetupGet(d => d.Value)
                .Returns(expected);
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, new [] {defaultValueAttributeMock.Object});

            var source = new DefaultValueAttributeSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
            defaultValueAttributeMock.VerifyAll();
        }

        [TestMethod]
        public void Get_Returns_Explicit_Null_From_DefaultValueAttribute()
        {
            const string name = "name";

            var defaultValueAttributeMock = new Mock<DefaultValueAttribute>(name);
            defaultValueAttributeMock.SetupGet(d => d.Value)
                .Returns((string?)null);
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, new [] {defaultValueAttributeMock.Object});

            var source = new DefaultValueAttributeSource();

            var result = source.Get(property, out var value);

            Assert.IsNull(value);
            Assert.IsTrue(result);
            defaultValueAttributeMock.VerifyAll();
        }

        [TestMethod]
        public void Get_Returns_Value_From_DefaultValueAttribute_Converted_To_String()
        {
            const string name = "name";
            const int expected = 1;
            const string expectedConverted = "1";

            var defaultValueAttributeMock = new Mock<DefaultValueAttribute>(name);
            defaultValueAttributeMock.SetupGet(d => d.Value)
                .Returns(expected);
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, new [] {defaultValueAttributeMock.Object});

            var source = new DefaultValueAttributeSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expectedConverted, value);
            Assert.IsTrue(result);
            defaultValueAttributeMock.VerifyAll();
        }
    }
}