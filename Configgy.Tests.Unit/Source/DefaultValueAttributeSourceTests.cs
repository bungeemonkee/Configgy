using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultValueAttributeValueSourceTests
    {
        [TestMethod]
        public void GetRawValue_Returns_Null_When_No_PropertyInfo()
        {
            var source = new DefaultValueAttributeSource();

            var result = source.GetRawValue("something", null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Value_From_DefaultValueAttribute()
        {
            const string name = "name";
            const string value = "1";

            var defaultValueAttributeMock = new Mock<DefaultValueAttribute>(name);
            defaultValueAttributeMock.SetupGet(d => d.Value)
                .Returns(value);

            var propertyInfoMock = new Mock<PropertyInfo>();
            propertyInfoMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] { defaultValueAttributeMock.Object });

            var source = new DefaultValueAttributeSource();

            var result = source.GetRawValue(name, propertyInfoMock.Object);

            defaultValueAttributeMock.VerifyGet(d => d.Value, Times.Once);
            propertyInfoMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            Assert.AreEqual(value, result);
        }
    }
}
