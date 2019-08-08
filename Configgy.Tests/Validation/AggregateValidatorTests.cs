using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public void Validate_Calls_Type_Validator()
        {
            const string value = "value";
            const string name = "name";
            var result = "something";
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            validatorMock.Setup(v => v.Validate(property, value, out result))
                .Returns(true);

            var validator = new AggregateValidator(new Dictionary<Type, IValueValidator>
            {
                [typeof(string)] = validatorMock.Object
            });

            validator.Validate(property, value, out result);

            validatorMock.VerifyAll();
        }

        [TestMethod]
        public void Validate_Calls_Property_Validator_Attribute()
        {
            const string value = "value";
            const string name = "name";

            IConfigProperty property = null;
            
            var result = value;

            var validatorMockAttribute = new Mock<Attribute>(MockBehavior.Strict);
            validatorMockAttribute.Setup(x => x.GetHashCode())
                .Returns(0);
            var validatorMock = validatorMockAttribute.As<IValueValidator>();
            validatorMock.Setup(v => v.Validate(It.Is<IConfigProperty>(x => x == property), value, out result))
                .Returns(true);
            
            property = new ConfigProperty(name, typeof(string), null, new[] {validatorMockAttribute.Object});

            var validator = new AggregateValidator(new Dictionary<Type, IValueValidator>());

            var coerced = validator.Validate(property, value, out result);

            Assert.AreEqual(value, result);
            Assert.IsTrue(coerced);
            validatorMock.VerifyAll();
        }
    }
}