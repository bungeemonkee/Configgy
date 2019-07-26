using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Configgy.Tests.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Configgy.Exceptions;
using Configgy.Source;
using Configgy.Transformation;
using Configgy.Validation;
using Configgy.Coercion;

namespace Configgy.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigExtensionsTests
    {
        [TestMethod]
        public void Validate_Calls_Source_Get_Transformer_Transform_Validator_Validate_And_Coercer_Coerce()
        {
            const string name = ConfigWrapper<int>.ThePropertyName;

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.Get(name, It.IsAny<PropertyInfo>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>();
            coercerMock.Setup(c => c.Coerce(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object,
                coercerMock.Object);

            config.Validate();

            sourceMock.Verify(s => s.Get(name, It.IsAny<PropertyInfo>(), out expectedRaw), Times.Once);
            transformerMock.Verify(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
            validatorMock.Verify(v => v.Validate(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue),
                Times.Once);
            coercerMock.Verify(c => c.Coerce(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue),
                Times.Once);
        }

        [TestMethod]
        public void Validate_Calls_Nothing_With_No_Properties()
        {
            string expectedRaw;
            int expectedValue;

            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();

            var transformerMock = new Mock<IValueTransformer>();

            var validatorMock = new Mock<IValueValidator>();

            var coercerMock = new Mock<IValueCoercer>();

            var config = new ConfigWrapperWithNoProperties(cache, sourceMock.Object, transformerMock.Object,
                validatorMock.Object, coercerMock.Object);

            config.Validate();

            sourceMock.Verify(s => s.Get(It.IsAny<string>(), It.IsAny<PropertyInfo>(), out expectedRaw), Times.Never);
            transformerMock.Verify(x => x.Transform(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>()),
                Times.Never);
            validatorMock.Verify(
                v => v.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>(), out expectedValue),
                Times.Never);
            coercerMock.Verify(
                c => c.Coerce(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>(), out expectedValue),
                Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigValidationException))]
        public void Validate_Throws_ConfigValidationException_When_A_Property_Is_Invalid()
        {
            const string name = ConfigWrapper<int>.ThePropertyName;

            var expectedRaw = "blorb";
            int expectedValue;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.Get(name, It.IsAny<PropertyInfo>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue))
                .Callback(() => { throw new Exception(); });

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object,
                null);

            config.Validate();
        }

        [TestMethod]
        public void GetCommandLineHelp_Returns_Help_Text()
        {
            var config = new ConfigWrapperWithHelpAttributes();

            var result = config.GetCommandLineHelp();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Populate_Does_Nothing_If_No_Properties()
        {
            var providerMock = new Mock<IConfigProvider>(MockBehavior.Strict);
            
            var config = new object();
            config.Populate(providerMock.Object);
        }

        [TestMethod]
        public void Populate_Sets_Properties_From_ConfigProvider()
        {
            const int intValue = 2;
            const string stringValue = "Alabamama";
            
            var providerMock = new Mock<IConfigProvider>(MockBehavior.Strict);
            providerMock.Setup(x => x.Get(nameof(TestWriteableConfig.Integer), It.IsAny<PropertyInfo>(), typeof(int)))
                .Returns(intValue);
            providerMock.Setup(x => x.Get(nameof(TestWriteableConfig.String), It.IsAny<PropertyInfo>(), typeof(string)))
                .Returns(stringValue);
            
            var config = new TestWriteableConfig();
            config.Populate(providerMock.Object);
            
            Assert.AreEqual(intValue, config.Integer);
            Assert.AreEqual(stringValue, config.String);
            providerMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigPopulationException))]
        public void Populate_Handles_Exceptions()
        {
            const int intValue = 2;
            const string stringValue = "Alabamama";
            
            var providerMock = new Mock<IConfigProvider>(MockBehavior.Strict);
            providerMock.Setup(x => x.Get(nameof(TestWriteableConfig.Integer), It.IsAny<PropertyInfo>(), typeof(int)))
                .Throws(new Exception("Whoopsie!"));
            providerMock.Setup(x => x.Get(nameof(TestWriteableConfig.String), It.IsAny<PropertyInfo>(), typeof(string)))
                .Returns(stringValue);
            
            var config = new TestWriteableConfig();
            config.Populate(providerMock.Object);
        }
    }
}