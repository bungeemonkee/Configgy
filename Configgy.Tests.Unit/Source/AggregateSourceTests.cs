using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    public class AggregateSourceTests
    {
        [TestMethod]
        public void GetRawValue_Returns_Value_From_First_Source_Without_Invoking_Second_Source()
        {
            const string name = "some value";
            const string value = "1";

            var sourceMock1 = new Mock<IValueSource>();
            sourceMock1.Setup(c => c.GetRawValue(name, null))
                .Returns(value);

            var sourceMock2 = new Mock<IValueSource>();

            var source = new AggregateSource(sourceMock1.Object, sourceMock2.Object);

            var result = source.GetRawValue(name, null);

            sourceMock1.Verify(c => c.GetRawValue(name, null), Times.Once);
            sourceMock2.Verify(c => c.GetRawValue(It.IsAny<string>(), It.IsAny<PropertyInfo>()), Times.Never);
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Value_From_Second_Source_When_First_Source_Returns_Null()
        {
            const string name = "some value";
            const string value = "1";

            var coercerMock1 = new Mock<IValueSource>();
            coercerMock1.Setup(c => c.GetRawValue(name, null))
                .Returns((string)null);

            var coercerMock2 = new Mock<IValueSource>();
            coercerMock2.Setup(c => c.GetRawValue(name, null))
                .Returns(value);

            var source = new AggregateSource(coercerMock1.Object, coercerMock2.Object);

            var result = source.GetRawValue(name, null);

            coercerMock1.Verify(c => c.GetRawValue(name, null), Times.Once);
            coercerMock2.Verify(c => c.GetRawValue(name, null), Times.Once);
            Assert.AreEqual(value, result);
        }
    }
}
