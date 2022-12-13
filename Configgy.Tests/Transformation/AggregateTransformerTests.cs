using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Configgy.Transformation;

namespace Configgy.Tests.Transformation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AggregateTransformerTests
    {
        [TestMethod]
        public void Transform_Invokes_All_Transformers_In_Order()
        {
            const string name = "some value";
            const string value = "1";
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, null);

            var transformerMock1 = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock1.Setup(x => x.Transform(property, value))
                .Returns(value);
            transformerMock1.Setup(x => x.Order)
                .Returns(20);

            var transformerMock2 = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock2.Setup(x => x.Transform(property, value))
                .Returns(value);
            transformerMock2.Setup(x => x.Order)
                .Returns(10);

            var transformer = new AggregateTransformer(transformerMock1.Object, transformerMock2.Object);

            var result = transformer.Transform(property, value);

            transformerMock1.VerifyAll();
            transformerMock1.VerifyAll();
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void Transform_Invokes_Property_Transformer()
        {
            const string name = "name";
            const string value = "1";

            var transformerMock1Attribute = new Mock<Attribute>(MockBehavior.Strict);
            transformerMock1Attribute.Setup(x => x.GetHashCode())
                .Returns(0);
            var transformerMock1 = transformerMock1Attribute.As<IValueTransformer>();

            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, new [] {transformerMock1Attribute.Object});

            transformerMock1.Setup(x => x.Transform(property, value))
                .Returns(value);
            transformerMock1.Setup(x => x.Order)
                .Returns(20);

            var transformerMock2 = new Mock<IValueTransformer>(MockBehavior.Strict);
            transformerMock2.Setup(x => x.Transform(property, value))
                .Returns(value);
            transformerMock2.Setup(x => x.Order)
                .Returns(10);

            var transformer = new AggregateTransformer(transformerMock2.Object);

            var result = transformer.Transform(property, value);

            transformerMock1.VerifyAll();
            transformerMock2.VerifyAll();
            Assert.AreEqual(value, result);
        }
    }
}
