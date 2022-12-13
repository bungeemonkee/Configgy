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
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>();
            coercerMock.Setup(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            config.Validate();

            sourceMock.VerifyAll();
            transformerMock.VerifyAll();
            validatorMock.VerifyAll();
            coercerMock.VerifyAll();
        }

        [TestMethod]
        public void Validate_Calls_Nothing_With_No_Properties()
        {
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);

            var config = new ConfigWrapperWithNoProperties(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            config.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigValidationException))]
        public void Validate_Throws_ConfigValidationException_When_A_Property_Is_Invalid()
        {
            const string name = ConfigWrapper<int>.ThePropertyName;

            var expectedRaw = "blorb";
            int expectedValue;
            var cache = new TestingCache();
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");

            IConfigProperty property = new ConfigProperty(name, typeof(int), propertyMock.Object, null);

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.Get(property, out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.Transform(property, expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate(property, expectedRaw, out expectedValue))
                .Callback(() => throw new Exception());

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

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
