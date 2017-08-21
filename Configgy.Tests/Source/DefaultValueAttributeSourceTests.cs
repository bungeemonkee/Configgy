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
        public void Get_Returns_Null_When_No_ICustomAttributeProvider()
        {
            var source = new DefaultValueAttributeSource();

            var result = source.Get("something", null, out string value);

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

            var propertyInfoMock = new Mock<PropertyInfo>();
            var attributeProviderMock = propertyInfoMock.As<ICustomAttributeProvider>();
            attributeProviderMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] {defaultValueAttributeMock.Object});

            var source = new DefaultValueAttributeSource();

            var result = source.Get(name, propertyInfoMock.Object, out string value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
            defaultValueAttributeMock.VerifyGet(d => d.Value, Times.AtLeastOnce);
            attributeProviderMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
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

            var propertyInfoMock = new Mock<PropertyInfo>();
            var attributeProviderMock = propertyInfoMock.As<ICustomAttributeProvider>();
            attributeProviderMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] {defaultValueAttributeMock.Object});

            var source = new DefaultValueAttributeSource();

            var result = source.Get(name, propertyInfoMock.Object, out string value);

            Assert.AreEqual(expectedConverted, value);
            Assert.IsTrue(result);
            defaultValueAttributeMock.VerifyGet(d => d.Value, Times.AtLeastOnce);
            attributeProviderMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
        }
    }
}