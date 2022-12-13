using System;
using System.Reflection;
using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Transformation;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Configgy.Tests
{
    [TestClass]
    public class ConfigProviderTests
    {
        [TestMethod]
        public void Get_Untyped_Gets_Value_From_Cache()
        {
            const string valueName = "value name";
            const string valueResult = "value result";
            
            var cacheMock = new Mock<IValueCache>(MockBehavior.Strict);
            cacheMock.Setup(x => x.Get(valueName, It.IsAny<Func<string, object>>()))
                .Returns(valueResult);
            
            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            var transformerMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            var validatorMock = new Mock<IValueValidator>(MockBehavior.Strict);
            var coercerMock = new Mock<IValueCoercer>(MockBehavior.Strict);

            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);

            var config = new ConfigProvider(cacheMock.Object, sourceMock.Object, transformerMock.Object, validatorMock.Object, coercerMock.Object);

            var result = config.Get(valueName, propertyMock.Object, typeof(string));
            
            Assert.AreEqual(valueResult, result);
            cacheMock.VerifyAll();
        }

        [TestMethod]
        public void Get_Untyped_Gets_Value_From_Source()
        {
            const string valueName = "value name";
            var valueResult = "value result";

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(x => x.Get(It.IsNotNull<IConfigProperty>(), out valueResult))
                .Returns(true);

            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            var config = new ConfigProvider(new DictionaryCache(), sourceMock.Object, new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer());

            var result = config.Get(valueName, propertyMock.Object, typeof(string));
            
            Assert.AreEqual(valueResult, result);
            sourceMock.VerifyAll();
        }

        [TestMethod]
        public void Attribute_Added_To_ConfigProvider_Is_Used()
        {
            const string valueName = "value name";
            var valueResult = "value result";
            var expectedResult = "actual result";

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(x => x.Get(It.IsNotNull<IConfigProperty>(), out valueResult))
                .Returns(true);

            var config = new ConfigProvider(new DictionaryCache(), sourceMock.Object, new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer());

            var attributeMock = new Mock<IValueTransformer>(MockBehavior.Strict);
            attributeMock.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), valueResult))
                .Returns(expectedResult);
            attributeMock.Setup(x => x.Order)
                .Returns(0);
            
            config.AddAttribute(valueName, attributeMock.Object);
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            var result = config.Get(valueName, propertyMock.Object, typeof(string));
            
            Assert.AreEqual(expectedResult, result);
            sourceMock.VerifyAll();
        }

        [TestMethod]
        public void Multiple_Attributes_Added_To_ConfigProvider_Are_Used()
        {
            const string valueName = "value name";
            var valueResult = "value result";
            var expectedResult1 = "actual result the first";
            var expectedResult2 = "actual result the second";

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(x => x.Get(It.IsNotNull<IConfigProperty>(), out valueResult))
                .Returns(true);

            var config = new ConfigProvider(new DictionaryCache(), sourceMock.Object, new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer());

            var attributeMock1 = new Mock<IValueTransformer>(MockBehavior.Strict);
            attributeMock1.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), valueResult))
                .Returns(expectedResult1);
            attributeMock1.Setup(x => x.Order)
                .Returns(0);

            var attributeMock2 = new Mock<IValueTransformer>(MockBehavior.Strict);
            attributeMock2.Setup(x => x.Transform(It.IsNotNull<IConfigProperty>(), expectedResult1))
                .Returns(expectedResult2);
            attributeMock2.Setup(x => x.Order)
                .Returns(0);
            
            config.AddAttribute(valueName, attributeMock1.Object);
            config.AddAttribute(valueName, attributeMock2.Object);
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            var result = config.Get(valueName, propertyMock.Object, typeof(string));
            
            Assert.AreEqual(expectedResult2, result);
            sourceMock.VerifyAll();
        }

        [TestMethod]
        public void Value_Name_Used_If_No_Source_Has_Alternate_Name()
        {
            const string valueName = "value name";
            const string alternateName = "not value name";
            var valueResult = "value result";
            var alternateResult = (string?) null;

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(x => x.Get(It.Is<IConfigProperty>(y => y.ValueName == valueName), out valueResult))
                .Returns(true);
            sourceMock.Setup(x => x.Get(It.Is<IConfigProperty>(y => y.ValueName == alternateName), out alternateResult))
                .Returns(false);

            var config = new ConfigProvider(new DictionaryCache(), sourceMock.Object, new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer());

            var attribute = new AlternateNameAttribute(alternateName);
            
            config.AddAttribute(valueName, attribute);
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            var result = config.Get(valueName, propertyMock.Object, typeof(string));
            
            Assert.AreEqual(valueResult, result);
            sourceMock.VerifyAll();
        }
    }
}