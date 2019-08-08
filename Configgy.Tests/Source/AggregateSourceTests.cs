using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Configgy.Source;

namespace Configgy.Tests.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AggregateSourceTests
    {
        [TestMethod]
        public void Get_Returns_Value_From_First_Source_Without_Invoking_Second_Source()
        {
            const string name = "some value";
            const string expected = "1";
            var value = expected;

            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);            
            
            var sourceMock1 = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock1.Setup(c => c.Get(property, out value))
                .Returns(true);

            var sourceMock2 = new Mock<IValueSource>(MockBehavior.Strict);

            var source = new AggregateSource(sourceMock1.Object, sourceMock2.Object);

            var result = source.Get(property, out value);

            sourceMock1.VerifyAll();
            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_Second_Source_When_First_Source_Returns_Null()
        {
            const string name = "some value";
            const string expected = "1";
            var value = expected;

            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);     

            var sourceMock1 = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock1.Setup(c => c.Get(property, out value))
                .Returns(false);

            var sourceMock2 = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock2.Setup(c => c.Get(property, out value))
                .Returns(true);

            var source = new AggregateSource(sourceMock1.Object, sourceMock2.Object);

            var result = source.Get(property, out value);

            sourceMock1.VerifyAll();
            sourceMock2.VerifyAll();
            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Ignores_Source_In_PreventSourceAttribute()
        {
            const string name = "some value";

            var preventSourceAttribute = new PreventSourceAttribute(typeof(ValueSourceStub));

            IConfigProperty property = new ConfigProperty(name, typeof(string), null, new []{preventSourceAttribute});

            var sourceStub = new ValueSourceStub();

            var source = new AggregateSource(sourceStub);

            var result = source.Get(property, out var value);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_Handles_Null_Property_Argument()
        {
            const string name = "some value";
            var expected = "value";

            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);     

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(c => c.Get(property, out expected))
                .Returns(true);

            var source = new AggregateSource(sourceMock.Object);

            var result = source.Get(property, out var value);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void Get_Returns_Value_From_Source_As_Attribute()
        {
            const string name = "some value";
            var expected = "value";

            ConfigProperty property = null;   

            var sourceMock = new Mock<IValueSource>(MockBehavior.Strict);
            sourceMock.Setup(c => c.Get(It.Is<ConfigProperty>(x => x == property), out expected))
                .Returns(true);
            
            property = new ConfigProperty(name, typeof(string), null, new [] {sourceMock.Object});

            var source = new AggregateSource();

            var result = source.Get(property, out var value);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, value);
            sourceMock.VerifyAll();
        }
    }
}