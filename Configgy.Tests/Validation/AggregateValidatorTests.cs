using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AggregateValidatorTests
    {
        [TestMethod]
        public void Validate_Does_Not_Fail_If_The_ICustomAttributeProvider_Parameter_Is_Null()
        {
            var validator = new AggregateValidator(new Dictionary<Type, IValueValidator>());

            string result;
            validator.Validate("value", "name", null, out result);
        }

        [TestMethod]
        public void Validate_Calls_Type_Validator()
        {
            const string value = "value";
            const string name = "name";

            string result;

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(v => v.Validate(value, name, null, out result));

            var validator = new AggregateValidator(new Dictionary<Type, IValueValidator>
            {
                [typeof(string)] = validatorMock.Object
            });

            validator.Validate(value, name, null, out result);

            validatorMock.Verify(v => v.Validate(value, name, null, out result), Times.Once);
        }

        [TestMethod]
        public void Validate_Calls_Property_Validator_Attribute()
        {
            const string value = "value";
            const string name = "name";

            var result = value;

            var validatorMockAttribute = new Mock<Attribute>();
            var validatorMock = validatorMockAttribute.As<IValueValidator>();
            validatorMock.Setup(v => v.Validate(value, name, It.IsAny<ICustomAttributeProvider>(), out result))
                .Returns(true);

            var ICustomAttributeProviderMock = new Mock<ICustomAttributeProvider>();
            ICustomAttributeProviderMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] {validatorMockAttribute.Object});

            var validator = new AggregateValidator(new Dictionary<Type, IValueValidator>());

            var coerced = validator.Validate(value, name, ICustomAttributeProviderMock.Object, out result);

            ICustomAttributeProviderMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            validatorMock.Verify(v => v.Validate(value, name, ICustomAttributeProviderMock.Object, out result),
                Times.Once);
            Assert.AreEqual(value, result);
            Assert.IsTrue(coerced);
        }
    }
}