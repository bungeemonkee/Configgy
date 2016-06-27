using Configgy.Coercion;
using Configgy.Exceptions;
using Configgy.Source;
using Configgy.Tests.Unit.Cache;
using Configgy.Transfomers;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Configgy.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigExtensionsTests
    {
        [TestMethod]
        public void Validate_Calls_Source_GetRawValue_Transformer_TransformValue_Validator_Validate_And_Coercer_Coerce()
        {
            const string name = ConfigWrapper<int>.ThePropertyName;

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.GetRawValue(name, It.IsAny<PropertyInfo>()))
                    .Returns(expectedRaw);

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.TransformValue(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate<int>(expectedRaw, name, It.IsAny<PropertyInfo>()));

            var coercerMock = new Mock<IValueCoercer>();
            coercerMock.Setup(c => c.CoerceTo<int>(expectedRaw, name, It.IsAny<PropertyInfo>()))
                    .Returns(expectedValue);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            config.Validate();

            sourceMock.Verify(s => s.GetRawValue(name, It.IsAny<PropertyInfo>()), Times.Once);
            transformerMock.Verify(x => x.TransformValue(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
            validatorMock.Verify(v => v.Validate<int>(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
            coercerMock.Verify(c => c.CoerceTo<int>(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
        }

        [TestMethod]
        public void Validate_Calls_Nothing_With_No_Properties()
        {
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();

            var transformerMock = new Mock<IValueTransformer>();

            var validatorMock = new Mock<IValueValidator>();

            var coercerMock = new Mock<IValueCoercer>();

            var config = new ConfigWrapperWithNoProperties(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            config.Validate();

            sourceMock.Verify(s => s.GetRawValue(It.IsAny<string>(), It.IsAny<PropertyInfo>()), Times.Never);
            transformerMock.Verify(x => x.TransformValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>()), Times.Never);
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

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.TransformValue(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate<int>(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Callback(() => { throw new Exception(); });

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, null);

            config.Validate();
        }

        [TestMethod]
        public void GetCommandLineHelp_Returns_Help_Text()
        {
            const string expected =
@"A program that does things. It's so good. Guys, you have no idea. Lorem Ipsum
is simply dummy text of the printing and typesetting industry. Lorem Ipsum has
been the industry's standard dummy text ever since the 1500s, when an unknown
printer took a galley of type and scrambled it to make a type specimen book. It
has survived not only five centuries, but also the leap into electronic
typesetting, remaining essentially unchanged. It was popularised in the 1960s
with the release of Letraset sheets containing Lorem Ipsum passages, and more
recently with desktop publishing software like Aldus PageMaker including
versions of Lorem Ipsum.

--EnumSetting=<TestingEnum:TestEnumTheFirst,TestEnumTheSecond,TestEnumTheThird>
        Some enum value that means something.

--JulieDoTheThing OR --JulieDoTheThing=<Boolean>

--SomeBytes=<Byte[]>
        A comma-delimited list of bytes.
        VeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongWord.

--SomeNumber=<Int32>
        This is a number.

--TextualizedParameter=<String>
        This is going to be very long help text. Lorem Ipsum is simply dummy
        text of the printing and typesetting industry. Lorem Ipsum has been the
        industry's standard dummy text ever since the 1500s, when an unknown
        printer took a galley of type and scrambled it to make a type specimen
        book. It has survived not only five centuries, but also the leap into
        electronic typesetting, remaining essentially unchanged. It was
        popularised in the 1960s with the release of Letraset sheets containing
        Lorem Ipsum passages, and more recently with desktop publishing
        software like Aldus PageMaker including versions of Lorem Ipsum.
";

            var config = new ConfigWrapperWithHelpAttributes();

            var result = config.GetCommandLineHelp();

            Assert.AreEqual(expected, result);
        }
    }
}
