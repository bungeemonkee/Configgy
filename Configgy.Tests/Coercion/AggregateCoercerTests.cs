using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System;
using System.Linq;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class AggregateCoercerTests
    {
        [TestMethod]
        public void Coerce_Returns_Value_From_First_Coercer_Without_Invoking_Second_Coercer()
        {
            const string name = "some value";
            const string value = "1";
            const int expected = 1;

            var result = expected;

            var coercerMock1 = new Mock<IValueCoercer>();
            coercerMock1.Setup(c => c.Coerce(value, name, null, out result))
                .Returns(true);

            var coercerMock2 = new Mock<IValueCoercer>();

            var coercer = new AggregateCoercer(coercerMock1.Object, coercerMock2.Object);

            var coerced = coercer.Coerce(value, name, null, out result);

            coercerMock1.Verify(c => c.Coerce(value, name, null, out result), Times.Once);
            coercerMock2.Verify(c => c.Coerce(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>(), out result), Times.Never);
            Assert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void Coerce_Returns_Value_From_Second_Coercer_When_First_Coercer_Returns_Null()
        {
            const string name = "some value";
            const string value = "1";
            const int expected = 1;
            var result = expected;
            var invalidResult = 0;

            var coercerMock1 = new Mock<IValueCoercer>();
            coercerMock1.Setup(c => c.Coerce(value, name, null, out invalidResult))
                .Returns(false);

            var coercerMock2 = new Mock<IValueCoercer>();
            coercerMock2.Setup(c => c.Coerce(value, name, null, out result))
                .Returns(true);

            var coercer = new AggregateCoercer(coercerMock1.Object, coercerMock2.Object);

            var coerced = coercer.Coerce(value, name, null, out result);

            coercerMock1.Verify(c => c.Coerce(value, name, null, out invalidResult), Times.Once);
            coercerMock2.Verify(c => c.Coerce(value, name, null, out result), Times.Once);
            Assert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void Coerce_Returns_Value_From_Property_Coercer_Attribute_Without_Invoking_Coerce_On_Other_Coercers()
        {
            const string name = "name";
            const string value = "1";
            const int expected = 1;
            var result = expected;
            var invalidResult = 0;
            
            var coercerMock1Attribute = new Mock<Attribute>();
            var coercerMock1 = coercerMock1Attribute.As<IValueCoercer>();
            coercerMock1.Setup(c => c.Coerce(value, name, It.IsAny<PropertyInfo>(), out result))
                .Returns(true);

            var coercerMock2 = new Mock<IValueCoercer>();

            var propertyInfoMock = new Mock<PropertyInfo>();
            var attributeProviderMock = propertyInfoMock.As<ICustomAttributeProvider>();
            attributeProviderMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() =>  new object[] { coercerMock1Attribute.Object });
            
            var coercer = new AggregateCoercer(coercerMock2.Object);

            var coerced = coercer.Coerce(value, name, propertyInfoMock.Object, out result);

            attributeProviderMock.Verify(p => p.GetCustomAttributes(true), Times.Once);
            coercerMock1.Verify(c => c.Coerce(value, name, propertyInfoMock.Object, out result), Times.Once);
            coercerMock2.Verify(c => c.Coerce(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PropertyInfo>(), out invalidResult), Times.Never);
            Assert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void Coerce_Returns_False_When_No_Internal_Coercers()
        {
            const string name = "name";
            const string value = "1";

            var coercer = new AggregateCoercer(new IValueCoercer[0]);

            string result;
            var coerced = coercer.Coerce(value, name, null, out result);
            
            Assert.IsNull(result);
            Assert.IsFalse(coerced);
        }
    }
}
