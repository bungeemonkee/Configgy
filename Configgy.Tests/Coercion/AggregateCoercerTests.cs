using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Configgy.Coercion;

namespace Configgy.Tests.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AggregateCoercerTests
    {
        [TestMethod]
        public void Coerce_Returns_Value_From_First_Coercer_Without_Invoking_Second_Coercer()
        {
            const string name = "some value";
            const string value = "1";
            const int expected = 1;
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var result = expected;

            var coercerMock1 = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock1.Setup(c => c.Coerce(property, value, out result))
                .Returns(true);

            var coercerMock2 = new Mock<IValueCoercer>(MockBehavior.Strict);

            var coercer = new AggregateCoercer(coercerMock1.Object, coercerMock2.Object);

            var coerced = coercer.Coerce(property, value, out result);

            coercerMock1.VerifyAll();
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
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var coercerMock1 = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock1.Setup(c => c.Coerce(property, value, out invalidResult))
                .Returns(false);

            var coercerMock2 = new Mock<IValueCoercer>(MockBehavior.Strict);
            coercerMock2.Setup(c => c.Coerce(property, value, out result))
                .Returns(true);

            var coercer = new AggregateCoercer(coercerMock1.Object, coercerMock2.Object);

            var coerced = coercer.Coerce(property, value, out result);

            coercerMock1.VerifyAll();
            coercerMock2.VerifyAll();
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

            ConfigProperty property = null!;
            
            var coercerMock1Attribute = new Mock<Attribute>(MockBehavior.Strict);
            var coercerMock1 = coercerMock1Attribute.As<IValueCoercer>();
            coercerMock1.Setup(c => c.Coerce(It.Is<ConfigProperty>(x => x == property), value, out result))
                .Returns(true);
            
            property = new ConfigProperty(name, TestUtilities.NullableProperty, new []{coercerMock1Attribute.Object});

            var coercerMock2 = new Mock<IValueCoercer>(MockBehavior.Strict);

            var coercer = new AggregateCoercer(coercerMock2.Object);

            var coerced = coercer.Coerce(property, value, out result);

            coercerMock1.VerifyAll();
            Assert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void Coerce_Returns_False_When_No_Internal_Coercers()
        {
            const string name = "name";
            const string value = "1";
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var coercer = new AggregateCoercer(new IValueCoercer[0]);

            var coerced = coercer.Coerce(property, value, out string result);

            Assert.IsNull(result);
            Assert.IsFalse(coerced);
        }
    }
}