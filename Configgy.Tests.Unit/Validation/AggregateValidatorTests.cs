using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Configgy.Tests.Unit.Validation
{
    [TestClass]
    public class AggregateValidatorTests
    {
        [TestMethod]
        public void Validate_Does_Not_Fail_If_The_PropertyInfo_Parameter_Is_Null()
        {
            var validator = new AggregateValidator(new Dictionary<Type, IValueValidator>());

            validator.Validate<string>("value", "name", null);
        }

        [TestMethod]
        public void Validate_Calls_Type_Validator()
        {
            var value = "value";
            var name = "name";

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(v => v.Validate<string>(value, name, null));

            var validator = new AggregateValidator(new Dictionary<Type, IValueValidator>
            {
                [typeof(string)] = validatorMock.Object
            });

            validator.Validate<string>(value, name, null);

            validatorMock.Verify(v => v.Validate<string>(value, name, null), Times.Once);
        }

        [TestMethod]
        public void Validate_Calls_Property_Validator_Attribute()
        {
            var value = "value";
            var name = "name";

            var validatorMock = new Mock<IValueValidator>();
            validatorMock.Setup(v => v.Validate<string>(value, name, It.IsAny<PropertyInfo>()));

            var propertyInfoMock = new Mock<PropertyInfo>();
            propertyInfoMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] { validatorMock.Object });

            var validator = new AggregateValidator(new Dictionary<Type, IValueValidator>());

            validator.Validate<string>(value, name, propertyInfoMock.Object);

            propertyInfoMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            validatorMock.Verify(v => v.Validate<string>(value, name, propertyInfoMock.Object), Times.Once);
        }
    }
}
