using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class AggregateSourceTests
    {
        [TestMethod]
        public void Get_Returns_Value_From_First_Source_Without_Invoking_Second_Source()
        {
            const string name = "some value";
            const string expected = "1";
            var value = expected;
            
            var sourceMock1 = new Mock<IValueSource>();
            sourceMock1.Setup(c => c.Get(name, null, out value))
                .Returns(true);

            var sourceMock2 = new Mock<IValueSource>();

            var source = new AggregateSource(sourceMock1.Object, sourceMock2.Object);

            var result = source.Get(name, null, out value);

            sourceMock1.Verify(c => c.Get(name, null, out value), Times.Once);
            sourceMock2.Verify(c => c.Get(It.IsAny<string>(), It.IsAny<PropertyInfo>(), out value), Times.Never);
            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_Second_Source_When_First_Source_Returns_Null()
        {
            const string name = "some value";
            const string expected = "1";
            var value = expected;

            var sourceMock1 = new Mock<IValueSource>();
            sourceMock1.Setup(c => c.Get(name, null, out value))
                .Returns(false);

            var sourceMock2 = new Mock<IValueSource>();
            sourceMock2.Setup(c => c.Get(name, null, out value))
                .Returns(true);

            var source = new AggregateSource(sourceMock1.Object, sourceMock2.Object);

            var result = source.Get(name, null, out value);

            sourceMock1.Verify(c => c.Get(name, null, out value), Times.Once);
            sourceMock2.Verify(c => c.Get(name, null, out value), Times.Once);
            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Ignores_Source_In_PreventSourceAttribute()
        {
            const string name = "some value";
            string value;

            var preventSourceAttribute = new PreventSourceAttribute(typeof(ValueSourceStub));

            var propertyInfoMock = new Mock<PropertyInfo>();
            var attributeProviderMock = propertyInfoMock.As<ICustomAttributeProvider>();
            attributeProviderMock.Setup(p => p.GetCustomAttributes(true))
                .Returns(() => new object[] { preventSourceAttribute });

            var sourceStub = new ValueSourceStub();

            var source = new AggregateSource(sourceStub);

            var result = source.Get(name, propertyInfoMock.Object, out value);

            Assert.IsFalse(result);
            attributeProviderMock.VerifyAll();
        }
    }
}
