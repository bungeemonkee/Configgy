using Configgy.Transfomers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    public class AggregateTransformerTests
    {
        [TestMethod]
        public void TransformValue_Invokes_All_Transformers_In_Order()
        {
            const string name = "some value";
            const string value = "1";

            var transformerMock1 = new Mock<IValueTransformer>();
            transformerMock1.Setup(x => x.TransformValue(value, name, null))
                .Returns(value);
            transformerMock1.Setup(x => x.Order)
                .Returns(20);

            var transformerMock2 = new Mock<IValueTransformer>();
            transformerMock2.Setup(x => x.TransformValue(value, name, null))
                .Returns(value);
            transformerMock2.Setup(x => x.Order)
                .Returns(10);

            var transformer = new AggregateTransformer(transformerMock1.Object, transformerMock2.Object);

            var result = transformer.TransformValue(value, name, null);

            transformerMock1.Verify(x => x.TransformValue(value, name, null), Times.Once);
            transformerMock1.Verify(x => x.TransformValue(value, name, null), Times.Once);
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void TransformValue_Invokes_Property_Transformer()
        {
            const string name = "name";
            const string value = "1";

            var transformerMock1 = new Mock<IValueTransformer>();
            transformerMock1.Setup(x => x.TransformValue(value, name, It.IsAny<PropertyInfo>()))
                .Returns(value);
            transformerMock1.Setup(x => x.Order)
                .Returns(20);

            var transformerMock2 = new Mock<IValueTransformer>();
            transformerMock2.Setup(x => x.TransformValue(value, name, It.IsAny<PropertyInfo>()))
                .Returns(value);
            transformerMock2.Setup(x => x.Order)
                .Returns(10);

            var propertyInfoMock = new Mock<PropertyInfo>();
            propertyInfoMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] { transformerMock1.Object });

            var transformer = new AggregateTransformer(transformerMock2.Object);

            var result = transformer.TransformValue(value, name, propertyInfoMock.Object);

            propertyInfoMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            transformerMock1.Verify(x => x.TransformValue(value, name, It.IsAny<PropertyInfo>()), Times.Once);
            transformerMock2.Verify(x => x.TransformValue(value, name, It.IsAny<PropertyInfo>()), Times.Once);
            Assert.AreEqual(value, result);
        }
    }
}
