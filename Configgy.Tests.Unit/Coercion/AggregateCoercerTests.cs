using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AggregateCoercerTests
    {
        [TestMethod]
        public void CoerceTo_Returns_Value_From_First_Coercer_Without_Invoking_Second_Coercer()
        {
            const string name = "some value";
            const string value = "1";
            const int expected = 1;

            var coercerMock1 = new Mock<IValueCoercer>();
            coercerMock1.Setup(c => c.CoerceTo<int>(value, name, null))
                .Returns(expected);

            var coercerMock2 = new Mock<IValueCoercer>();

            var coercer = new AggregateCoercer(coercerMock1.Object, coercerMock2.Object);

            var result = coercer.CoerceTo<int>(value, name, null);

            coercerMock1.Verify(c => c.CoerceTo<int>(value, name, null), Times.Once);
            coercerMock2.Verify(c => c.CoerceTo<int>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>()), Times.Never);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Value_From_Second_Coercer_When_First_Coercer_Returns_Null()
        {
            const string name = "some value";
            const string value = "1";
            const int expected = 1;

            var coercerMock1 = new Mock<IValueCoercer>();
            coercerMock1.Setup(c => c.CoerceTo<int>(value, name, null))
                .Returns((object)null);

            var coercerMock2 = new Mock<IValueCoercer>();
            coercerMock2.Setup(c => c.CoerceTo<int>(value, name, null))
                .Returns(expected);

            var coercer = new AggregateCoercer(coercerMock1.Object, coercerMock2.Object);

            var result = coercer.CoerceTo<int>(value, name, null);

            coercerMock1.Verify(c => c.CoerceTo<int>(value, name, null), Times.Once);
            coercerMock2.Verify(c => c.CoerceTo<int>(value, name, null), Times.Once);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Value_From_Property_Coercer_Attribute_Without_Invoking_CoerceTo_On_Other_Coercers()
        {
            const string name = "name";
            const string value = "1";
            const int expected = 1;
            
            var coercerMock1 = new Mock<IValueCoercer>();
            coercerMock1.Setup(c => c.CoerceTo<int>(value, name, It.IsAny<PropertyInfo>()))
                .Returns(expected);

            var coercerMock2 = new Mock<IValueCoercer>();

            var propertyInfoMock = new Mock<PropertyInfo>();
            propertyInfoMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] { coercerMock1.Object });

            var coercer = new AggregateCoercer(coercerMock2.Object);

            var result = (int)coercer.CoerceTo<int>(value, name, propertyInfoMock.Object);

            propertyInfoMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            coercerMock1.Verify(c => c.CoerceTo<int>(value, name, propertyInfoMock.Object), Times.Once);
            coercerMock2.Verify(c => c.CoerceTo<int>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>()), Times.Never);
            Assert.AreEqual(expected, result);
        }
    }
}
