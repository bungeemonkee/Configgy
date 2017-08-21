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

            var transformerMock1 = new Mock<IValueTransformer>();
            transformerMock1.Setup(x => x.Transform(value, name, null))
                .Returns(value);
            transformerMock1.Setup(x => x.Order)
                .Returns(20);

            var transformerMock2 = new Mock<IValueTransformer>();
            transformerMock2.Setup(x => x.Transform(value, name, null))
                .Returns(value);
            transformerMock2.Setup(x => x.Order)
                .Returns(10);

            var transformer = new AggregateTransformer(transformerMock1.Object, transformerMock2.Object);

            var result = transformer.Transform(value, name, null);

            transformerMock1.Verify(x => x.Transform(value, name, null), Times.Once);
            transformerMock1.Verify(x => x.Transform(value, name, null), Times.Once);
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void Transform_Invokes_Property_Transformer()
        {
            const string name = "name";
            const string value = "1";

            var transformerMock1Attribute = new Mock<Attribute>();
            var transformerMock1 = transformerMock1Attribute.As<IValueTransformer>();
            transformerMock1.Setup(x => x.Transform(value, name, It.IsAny<ICustomAttributeProvider>()))
                .Returns(value);
            transformerMock1.Setup(x => x.Order)
                .Returns(20);

            var transformerMock2 = new Mock<IValueTransformer>();
            transformerMock2.Setup(x => x.Transform(value, name, It.IsAny<ICustomAttributeProvider>()))
                .Returns(value);
            transformerMock2.Setup(x => x.Order)
                .Returns(10);

            var ICustomAttributeProviderMock = new Mock<ICustomAttributeProvider>();
            ICustomAttributeProviderMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] {transformerMock1Attribute.Object});

            var transformer = new AggregateTransformer(transformerMock2.Object);

            var result = transformer.Transform(value, name, ICustomAttributeProviderMock.Object);

            ICustomAttributeProviderMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            transformerMock1.Verify(x => x.Transform(value, name, It.IsAny<ICustomAttributeProvider>()), Times.Once);
            transformerMock2.Verify(x => x.Transform(value, name, It.IsAny<ICustomAttributeProvider>()), Times.Once);
            Assert.AreEqual(value, result);
        }
    }
}