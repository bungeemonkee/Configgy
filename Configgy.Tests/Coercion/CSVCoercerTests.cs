using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Coercion;

namespace Configgy.Tests.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CsvCoercerTests
    {
        [TestMethod]
        public void Coerce_Works_With_Array_Of_Int()
        {
            const string input = "1,4,78,222";
            var expected = new[] {1, 4, 78, 222};

            var coercer = new CsvCoercerAttribute(typeof(int));

            var coerced = coercer.Coerce(null, input, out int[] result);

            Assert.IsTrue(coerced);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Coerce_Works_With_Custom_Separator()
        {
            const string input = "1 | 4 | 78 | 222";
            var expected = new[] {1, 4, 78, 222};

            var coercer = new CsvCoercerAttribute(typeof(int), " | ");

            var coerced = coercer.Coerce(null, input, out int[] result);

            Assert.IsTrue(coerced);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Coerce_Returns_Null_With_Invalid_Type()
        {
            const string input = "1,4,78,222";

            var coercer = new CsvCoercerAttribute(typeof(int));

            var coerced = coercer.Coerce(null, input, out long[] result);

            Assert.IsFalse(coerced);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Coerce_Throws_Exception_With_Invalid_Values()
        {
            const string input = "1,4,78,abcdefg";

            var coercer = new CsvCoercerAttribute(typeof(int));

            coercer.Coerce(null, input, out int[] _);
        }

        [TestMethod]
        public void Coerce_Returns_Empty_Array_With_Empty_String()
        {
            const string input = "";

            var coercer = new CsvCoercerAttribute(typeof(int));

            var coerced = coercer.Coerce(null, input, out int[] result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int[]));
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void Coerce_Returns_Null_Array_With_Null_String()
        {
            const string input = null;

            var coercer = new CsvCoercerAttribute(typeof(int));

            var coerced = coercer.Coerce(null, input, out int[] result);

            Assert.IsTrue(coerced);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Coerce_Throws_Exception_For_Unconvertible_Type()
        {
            const string input = null;

            var coercer = new CsvCoercerAttribute(typeof(DescriptionAttribute));

            var coerced = coercer.Coerce(null, input, out DescriptionAttribute[] result);

            Assert.IsTrue(coerced);
            Assert.IsNull(result);
        }
    }
}