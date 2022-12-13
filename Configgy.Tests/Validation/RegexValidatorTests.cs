using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;
using Moq;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegexValidatorTests
    {
        [TestMethod]
        public void Validate_Allows_Valid_Value()
        {
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty("testing", typeof(string), propertyMock.Object, null);
            
            var validator = new RegexValidatorAttribute("aaabbbccc");

            validator.Validate(property, "aaabbbccc", out string value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Validate_Throws_Exception_For_Invalid_Value()
        {
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            
            IConfigProperty property = new ConfigProperty("testing", typeof(string), propertyMock.Object, null);
            
            var validator = new RegexValidatorAttribute("aaabbbccc");

            validator.Validate(property, "the number twelve", out string value);
        }
    }
}