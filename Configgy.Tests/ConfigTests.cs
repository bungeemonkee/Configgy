using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Cache;
using Moq;
using Configgy.Tests.Unit.Cache;
using Configgy.Source;
using System.Reflection;
using Configgy.Validation;
using Configgy.Coercion;
using Configgy.Exceptions;
using Configgy.Transformation;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
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
            var args = new[] { "argument1", "argument2" };

            var config = new ConfigWrapper<object>(args);

            Assert.IsNotNull(config);
        }

        [TestMethod]
        public void ClearCache_Calls_Cache_Clear()
        {
            var cacheMock = new Mock<IValueCache>();

            var config = new ConfigWrapper<object>(cacheMock.Object, null, null, null, null);

            config.ClearCache();

            cacheMock.Verify(c => c.Clear(), Times.Once);
        }

        [TestMethod]
        public void Get_Calls_Cache_Get()
        {
            const string name = "__value__";

            var expected = new object();

            var cacheMock = new Mock<IValueCache>();
            cacheMock.Setup(c => c.Get(name, It.IsAny<Func<string, object>>()))
                .Returns(expected);

            var config = new ConfigWrapper<object>(cacheMock.Object, null, null, null, null);

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

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.Get(name, null, out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate(expectedRaw, name, null, out expectedValue))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>();
            coercerMock.Setup(c => c.Coerce(expectedRaw, name, null, out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.Get_Wrapper(name);

            sourceMock.Verify(s => s.Get(name, null, out expectedRaw), Times.Once);
            transformerMock.Verify(x => x.Transform(expectedRaw, name, null), Times.Once);
            validatorMock.Verify(v => v.Validate(expectedRaw, name, null, out expectedValue), Times.Once);
            coercerMock.Verify(c => c.Coerce(expectedRaw, name, null, out expectedValue), Times.Once);
            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_But_Not_Coercer_Coerce_When_Callback_Invoked_And_Validator_Validate_Returns_A_Value()
        {
            const string name = "__value__";

            var expectedRaw = "1";
            var expectedValue = 1;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.Get(name, null, out expectedRaw))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()))
                .Returns(expectedRaw);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate(expectedRaw, name, null, out expectedValue))
                .Returns(true);

            var coercerMock = new Mock<IValueCoercer>();
            coercerMock.Setup(c => c.Coerce(expectedRaw, name, null, out expectedValue))
                .Returns(true);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.Get_Wrapper(name);

            sourceMock.Verify(s => s.Get(name, null, out expectedRaw), Times.Once);
            transformerMock.Verify(x => x.Transform(expectedRaw, name, null), Times.Once);
            validatorMock.Verify(v => v.Validate(expectedRaw, name, null, out expectedValue), Times.Once);
            coercerMock.Verify(c => c.Coerce(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>(), out expectedValue), Times.Never);
            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_But_Skips_Coercer_Coerce_When_Callback_Invoked_For_String()
        {
            const string name = "__value__";

            var expected = "bananas";
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.Get(name, null, out expected))
                .Returns(true);

            var transformerMock = new Mock<IValueTransformer>();
            transformerMock.Setup(x => x.Transform(expected, name, It.IsAny<PropertyInfo>()))
                .Returns(expected);

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(s => s.Validate(expected, name, null, out expected))
                .Returns(false);

            var coercerMock = new Mock<IValueCoercer>();

            var config = new ConfigWrapper<string>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.Get_Wrapper(name);

            sourceMock.Verify(s => s.Get(name, null, out expected), Times.Once);
            transformerMock.Verify(x => x.Transform(expected, name, null), Times.Once);
            validatorMock.Verify(v => v.Validate(expected, name, null, out expected), Times.Once);
            coercerMock.Verify(c => c.Coerce(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>(), out expected), Times.Never);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_And_Coercer_Coerce_When_Callback_Invoked_Via_Property()
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

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.TheProperty;

            sourceMock.Verify(s => s.Get(name, It.IsAny<PropertyInfo>(), out expectedRaw), Times.Once);
            transformerMock.Verify(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
            validatorMock.Verify(v => v.Validate(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue), Times.Once);
            coercerMock.Verify(c => c.Coerce(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue), Times.Once);
            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Get_Calls_Source_Get_Transformer_Transform_Validator_Validate_And_Coercer_Coerce_Once_Each_When_Callback_Invoked_Via_Property_Twice_When_Using_A_Real_Cache_Implementation()
        {
            const string name = ConfigWrapper<object>.ThePropertyName;

            var expectedRaw = "{}";
            var expectedValue = new object();
            var cache = new DictionaryCache();

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

            var config = new ConfigWrapper<object>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result1 = config.TheProperty;
            var result2 = config.TheProperty;

            sourceMock.Verify(s => s.Get(name, It.IsAny<PropertyInfo>(), out expectedRaw), Times.Once);
            transformerMock.Verify(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Once);
            validatorMock.Verify(v => v.Validate(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue), Times.Once);
            coercerMock.Verify(c => c.Coerce(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue), Times.Once);
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

            var config = new ConfigWrapper<object>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result1 = config.TheProperty;
            cache.Clear();
            var result2 = config.TheProperty;

            sourceMock.Verify(s => s.Get(name, It.IsAny<PropertyInfo>(), out expectedRaw), Times.Exactly(2));
            transformerMock.Verify(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Exactly(2));
            validatorMock.Verify(v => v.Validate(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue), Times.Exactly(2));
            coercerMock.Verify(c => c.Coerce(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue), Times.Exactly(2));
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

            var config = new ConfigWrapper<object>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result1 = config.TheProperty;
            cache.Remove(name);
            var result2 = config.TheProperty;

            sourceMock.Verify(s => s.Get(name, It.IsAny<PropertyInfo>(), out expectedRaw), Times.Exactly(2));
            transformerMock.Verify(x => x.Transform(expectedRaw, name, It.IsAny<PropertyInfo>()), Times.Exactly(2));
            validatorMock.Verify(v => v.Validate(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue), Times.Exactly(2));
            coercerMock.Verify(c => c.Coerce(expectedRaw, name, It.IsAny<PropertyInfo>(), out expectedValue), Times.Exactly(2));
            Assert.AreEqual(expectedValue, result1);
            Assert.AreSame(result1, result2);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingValueException))]
        public void Get_Throws_MissingValueException_When_Source_Get_Returns_Null()
        {
            const string name = "__value__";

            string expectedRaw;
            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.Get(name, null, out expectedRaw))
                .Returns(false);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, null, null, null);

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
                .Returns(false);

            var config = new ConfigWrapper<int>(cache, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            config.Get_Wrapper(name);
        }
    }
}
