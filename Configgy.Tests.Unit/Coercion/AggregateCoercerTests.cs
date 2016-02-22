using Configgy.Coercion;
using Configgy.Source;
using Configgy.Tests.Unit.Cache;
using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
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
            const string name = ConfigWrapperWithPropertyWithCoercerProperty<int[]>.ThePropertyName;
            const string value = "[1,2,5]";
            var expected = new int[] { 1, 2, 5 };

            var cache = new TestingCache();

            var sourceMock = new Mock<IValueSource>();
            sourceMock.Setup(s => s.GetRawValue(name, It.IsAny<PropertyInfo>()))
                .Returns(value);

            var validatorMock = new Mock<IValueValidator>();

            var coercerMock = new Mock<IValueCoercer>();

            var coercer = new AggregateCoercer(coercerMock.Object);

            var config = new ConfigWrapperWithPropertyWithCoercerProperty<int[]>(cache, sourceMock.Object, validatorMock.Object, coercer);

            var result = config.TheProperty;

            coercerMock.Verify(c => c.CoerceTo<int[]>(value, name, null), Times.Never);
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
