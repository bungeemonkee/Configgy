using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
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

            var sourceMock1 = new Mock<IValueSource>();
            sourceMock1.Setup(c => c.GetRawValue(name, null))
                .Returns((string)null);

            var sourceMock2 = new Mock<IValueSource>();
            sourceMock2.Setup(c => c.GetRawValue(name, null))
                .Returns(value);

            var source = new AggregateSource(sourceMock1.Object, sourceMock2.Object);

            var result = source.GetRawValue(name, null);

            sourceMock1.Verify(c => c.GetRawValue(name, null), Times.Once);
            sourceMock2.Verify(c => c.GetRawValue(name, null), Times.Once);
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Ignores_Source_In_PreventSourceAttribute()
        {
            const string name = "some value";

            var preventSourceAttribute = new PreventSourceAttribute(typeof(ValueSourceStub));

            var propertyInfoMock = new Mock<PropertyInfo>();
            propertyInfoMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] { preventSourceAttribute });

            var sourceStub = new ValueSourceStub();

            var source = new AggregateSource(sourceStub);

            var result = source.GetRawValue(name, propertyInfoMock.Object);

            propertyInfoMock.VerifyAll();
            Assert.IsNull(result);
        }
    }
}
