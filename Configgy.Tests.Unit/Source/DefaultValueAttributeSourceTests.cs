using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultValueAttributeValueSourceTests
    {
        [TestMethod]
        public void Get_Returns_Null_When_No_PropertyInfo()
        {
            var source = new DefaultValueAttributeSource();

            string value;
            var result = source.Get("something", null, out value);

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
            propertyInfoMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] { defaultValueAttributeMock.Object });

            var source = new DefaultValueAttributeSource();

            string value;
            var result = source.Get(name, propertyInfoMock.Object, out value);

            defaultValueAttributeMock.VerifyGet(d => d.Value, Times.AtLeastOnce);
            propertyInfoMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
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
            propertyInfoMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] { defaultValueAttributeMock.Object });

            var source = new DefaultValueAttributeSource();

            string value;
            var result = source.Get(name, propertyInfoMock.Object, out value);

            defaultValueAttributeMock.VerifyGet(d => d.Value, Times.AtLeastOnce);
            propertyInfoMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            Assert.AreEqual(expectedConverted, value);
            Assert.IsTrue(result);
        }
    }
}
