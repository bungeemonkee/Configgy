using System;
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

            var config = new ConfigProvider(cacheMock.Object, null, null, null, null);

            var result = config.Get(valueName, null, typeof(string));
            
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

            var config = new ConfigProvider(new DictionaryCache(), sourceMock.Object, new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer());

            var result = config.Get(valueName, null, typeof(string));
            
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
            
            var result = config.Get(valueName, null, typeof(string));
            
            Assert.AreEqual(expectedResult, result);
            sourceMock.VerifyAll();
        }
    }
}