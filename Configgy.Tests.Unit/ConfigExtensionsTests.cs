using Configgy.Coercion;
using Configgy.Exceptions;
using Configgy.Source;
using Configgy.Tests.Unit.Cache;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Reflection;

namespace Configgy.Tests.Unit
{
    [TestClass]
    public class ConfigExtensionsTests
    {
        [TestMethod]
        public void Validate_Calls_Source_GetRawValue_Validator_Validate_And_Coercer_Coerce()
        {
            const string name = ConfigWrapper<int>.ThePropertyName;

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.GetRawValue(name, It.IsAny<PropertyInfo>()))
                    .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate<int>(expectedRaw, name, It.IsAny<PropertyInfo>()));

            var coercerMock = new Mock<IValueCoercer>();
            coercerMock.Setup(c => c.CoerceTo<int>(expectedRaw, name, It.IsAny<PropertyInfo>()))
                    .Returns(expectedValue);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, validatorMock.Object, coercerMock.Object);

            config.Validate();

            sourceMock.Verify(s => s.GetRawValue(name, It.IsAny<PropertyInfo>()), Times.Once);
            validatorMock.Verify(v => v.Validate<int>(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
            coercerMock.Verify(c => c.CoerceTo<int>(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
        }

        [TestMethod]
        public void Validate_Calls_Nothing_With_No_Properties()
        {
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();

            var validatorMock = new Mock<IValueValidator>();

            var coercerMock = new Mock<IValueCoercer>();

            var config = new ConfigWrapperWithNoProperties(cache, sourceMock.Object, validatorMock.Object, coercerMock.Object);

            config.Validate();

            sourceMock.Verify(s => s.GetRawValue(It.IsAny<string>(), It.IsAny<PropertyInfo>()), Times.Never);
            validatorMock.Verify(v => v.Validate<int>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>()), Times.Never);
            coercerMock.Verify(c => c.CoerceTo<int>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigValidationException))]
        public void Validate_Throws_ConfigValidationException_When_A_Property_Is_Invalid()
        {
            const string name = ConfigWrapper<int>.ThePropertyName;

            var expectedRaw = "blorb";
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.GetRawValue(name, It.IsAny<PropertyInfo>()))
                    .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate<int>(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Callback(() => { throw new Exception(); });

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, validatorMock.Object, null);

            config.Validate();
        }
    }
}
