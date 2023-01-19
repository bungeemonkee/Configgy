﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Coercion;
using Moq;

namespace Configgy.Tests.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class XmlCoercerTests
    {
        [TestMethod]
        public void XmlCoercer_Coerce_Works_With_Array_Of_Int()
        {
            const string input = @"
<ArrayOfint xmlns=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
    <int>1</int>
    <int>4</int>
    <int>78</int>
    <int>222</int>
</ArrayOfint>
";

            var expected = new[] {1, 4, 78, 222};

            var coercer = new XmlCoercerAttribute();

            var propertyMock = new Mock<IConfigProperty>(MockBehavior.Strict);

            var coerced = coercer.Coerce(propertyMock.Object, input, out int[] result);

            CollectionAssert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void XmlCoercer_Coerce_Throws_Exception_With_Invalid_Xml()
        {
            const string input = @"
<ArrayOfint xmlns=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
    <int>1</int
    <int>4</int>
    <int>78</int>
    <int222</int>
</ArrayOfint>
";

            var coercer = new XmlCoercerAttribute();

            var propertyMock = new Mock<IConfigProperty>(MockBehavior.Strict);

            var coerced = coercer.Coerce(propertyMock.Object, input, out int[] result);

            Assert.IsNull(result);
            Assert.IsFalse(coerced);
        }

        [TestMethod]
        public void XmlCoercer_Coerce_Returns_Null_With_Null_Xml()
        {
            const string? input = null;

            var coercer = new XmlCoercerAttribute();

            var propertyMock = new Mock<IConfigProperty>(MockBehavior.Strict);

            var coerced = coercer.Coerce(propertyMock.Object, input, out int[] result);

            Assert.IsNull(result);
            Assert.IsTrue(coerced);
        }
    }
}