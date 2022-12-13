using System;
using System.Diagnostics.CodeAnalysis;
using Configgy.Tests.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Configgy.Exceptions;
using Configgy.Source;
using Configgy.Cache;
using Configgy.Validation;
using Configgy.Transformation;
using Configgy.Coercion;

namespace Configgy.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigTests
    {
        [TestMethod]
        public void Default_Constructor_Works()
        {
            var config = new ConfigWrapper<object>();

            Assert.IsNotNull(config);
        }

        [TestMethod]
        public void Constructor_With_String_Array_Works()
        {
            var args = new[] {"argument1", "argument2"};

            var config = new ConfigWrapper<object>(args);

            Assert.IsNotNull(config);
        }

        [TestMethod]
        public void Get_Calls_Cache_Get()
        {
            const string name = "__value__";

            var expected = new object();

            var cacheMock = new Mock<IValueCache>(MockBehavior.Strict);
            cacheMock.Setup(c => c.Get(name, It.IsAny<Func<string, object>>()))
                .Returns(expected);

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);
            
            var config = new ConfigWrapper<object>(cacheMock.Object, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.Get_Wrapper(name);

            cacheMock.Verify(c => c.Get(name, It.IsAny<Func<string, object>>()), Times.Once);
            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_TranmsformValue_Validator_Validate_And_Coercer_Coerce_When_Callback_Invoked()
        {
            const string name = "__value__";

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock.Setup(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.Get_Wrapper(name);

            sourceMock.VerifyAll();
            transformerMock.VerifyAll();
            validatorMock.VerifyAll();
            coercerMock.VerifyAll();
            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_But_Not_Coercer_Coerce_When_Callback_Invoked_And_Validator_Validate_Returns_A_Value()
        {
            const string name = "__value__";

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(true);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.Get_Wrapper(name);

            sourceMock.VerifyAll();
            transformerMock.VerifyAll();
            validatorMock.VerifyAll();
            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_But_Skips_Coercer_Coerce_When_Callback_Invoked_For_String()
        {
            const string name = "__value__";

            var expected = "bananas";
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expected))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expected))
                .Returns(expected);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expected, out expected))
                .Returns(true);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);

            var config = new ConfigWrapper<string>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.Get_Wrapper(name);

            sourceMock.VerifyAll();
            transformerMock.VerifyAll();
            validatorMock.VerifyAll();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_And_Coercer_Coerce_When_Callback_Invoked_Via_Property()
        {
            const string name = ConfigWrapper<int>.ThePropertyName;

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock.Setup(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.TheProperty;

            sourceMock.VerifyAll();
            transformerMock.VerifyAll();
            validatorMock.VerifyAll();
            coercerMock.VerifyAll();
            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_And_Coercer_Coerce_Once_Each_When_Callback_Invoked_Via_Property_Twice_When_Using_A_Real_Cache_Implementation()
        {
            const string name = ConfigWrapper<object>.ThePropertyName;

            var expectedRaw = "{}";
            var expectedValue = new object();
            var cache = new DictionaryCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock.Setup(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<object>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result1 = config.TheProperty;
            var result2 = config.TheProperty;

            sourceMock.Verify(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw), Times.Once);
            transformerMock.Verify(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw), Times.Once);
            validatorMock.Verify(v => v.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue), Times.Once);
            coercerMock.Verify(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue), Times.Once);
            Assert.AreEqual(expectedValue, result1);
            Assert.AreSame(result1, result2);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_And_Coercer_Coerce_Twice_Each_When_Callback_Invoked_Via_Property_Twice_When_Using_A_Real_Cache_Implementation_And_The_Cache_Is_Cleared_In_Between()
        {
            const string name = ConfigWrapper<object>.ThePropertyName;

            var expectedRaw = "{}";
            var expectedValue = new object();
            var cache = new DictionaryCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock.Setup(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<object>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result1 = config.TheProperty;
            cache.Remove();
            var result2 = config.TheProperty;

            sourceMock.Verify(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw), Times.Exactly(2));
            transformerMock.Verify(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw), Times.Exactly(2));
            validatorMock.Verify(v => v.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue), Times.Exactly(2));
            coercerMock.Verify(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue), Times.Exactly(2));
            Assert.AreEqual(expectedValue, result1);
            Assert.AreSame(result1, result2);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_And_Coercer_Coerce_Twice_Each_When_Callback_Invoked_Via_Property_Twice_When_Using_A_Real_Cache_Implementation_And_The_Property_Is_Removed_From_The_Cache_In_Between()
        {
            const string name = ConfigWrapper<object>.ThePropertyName;

            var expectedRaw = "{}";
            var expectedValue = new object();
            var cache = new DictionaryCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock.Setup(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<object>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result1 = config.TheProperty;
            cache.Remove(name);
            var result2 = config.TheProperty;

            sourceMock.Verify(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw), Times.Exactly(2));
            transformerMock.Verify(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw), Times.Exactly(2));
            validatorMock.Verify(v => v.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue), Times.Exactly(2));
            coercerMock.Verify(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue), Times.Exactly(2));
            Assert.AreEqual(expectedValue, result1);
            Assert.AreSame(result1, result2);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingValueException))]
        public void Get_Throws_MissingValueException_When_Source_Get_Returns_Null()
        {
            const string name = "__value__";

            string? expectedRaw;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(false);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);
            
            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            config.Get_Wrapper(name);
        }

        [TestMethod]
        [ExpectedException(typeof(CoercionException))]
        public void Get_Throws_CoercionException_When_Coercer_Coerce_Returns_False()
        {
            const string name = "__value__";

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(s => s.Get(It.IsNotNull<IConfigProperty>(), out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedRaw))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(s => s.Validate(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock.Setup(c => c.Coerce(It.IsNotNull<IConfigProperty>(), expectedRaw, out expectedValue))
                .Returns(false);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            config.Get_Wrapper(name);
        }
    }
}