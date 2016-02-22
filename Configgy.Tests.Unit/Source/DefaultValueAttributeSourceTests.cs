using Configgy.Coercion;
using Configgy.Source;
using Configgy.Tests.Unit.Cache;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
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
            const string name = ConfigWrapperWithPropertyWithefaultValueAttribute<int>.ThePropertyName;

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var source = new DefaultValueAttributeSource();

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate<int>(expectedRaw, name, It.IsAny<PropertyInfo>()));

            var coercerMock = new Mock<IValueCoercer>();
            coercerMock.Setup(c => c.CoerceTo<int>(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Returns(expectedValue);

            var config = new ConfigWrapperWithPropertyWithefaultValueAttribute<int>(cache, source, validatorMock.Object, coercerMock.Object);

            var result = config.TheProperty;
            
            validatorMock.Verify(v => v.Validate<int>(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
            coercerMock.Verify(c => c.CoerceTo<int>(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
            Assert.AreEqual(expectedValue, result);
        }
    }
}
