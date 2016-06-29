using System;
using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CsvCoercerTests
    {
        [TestMethod]
        public void Coerce_Works_With_Array_Of_Int()
        {
            const string input = "1,4,78,222";
            var expected = new [] { 1, 4, 78, 222 };

            var coercer = new CsvCoercerAttribute(typeof(int));

            int[] result;
            var coerced = coercer.Coerce(input, null, null, out result);

            Assert.IsTrue(coerced);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Coerce_Works_With_Custom_Separator()
        {
            const string input = "1 | 4 | 78 | 222";
            var expected = new int[] { 1, 4, 78, 222 };

            var coercer = new CsvCoercerAttribute(typeof(int), " | ");

            int[] result;
            var coerced = coercer.Coerce(input, null, null, out result);

            Assert.IsTrue(coerced);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Coerce_Returns_Null_With_Invalid_Type()
        {
            const string input = "1,4,78,222";

            var coercer = new CsvCoercerAttribute(typeof(int));

            long[] result;
            var coerced = coercer.Coerce(input, null, null, out result);

            Assert.IsFalse(coerced);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Coerce_Throws_Exception_With_Invalid_Values()
        {
            const string input = "1,4,78,abcdefg";

            var coercer = new CsvCoercerAttribute(typeof(int));

            int[] result;
            coercer.Coerce(input, null, null, out result);
        }

        [TestMethod]
        public void Coerce_Returns_Empty_Array_With_Empty_String()
        {
            const string input = "";

            var coercer = new CsvCoercerAttribute(typeof(int));

            int[] result;
            var coerced = coercer.Coerce(input, null, null, out result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int[]));
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void Coerce_Returns_Null_Array_With_Null_String()
        {
            const string input = null;

            var coercer = new CsvCoercerAttribute(typeof(int));

            int[] result;
            var coerced = coercer.Coerce(input, null, null, out result);

            Assert.IsTrue(coerced);
            Assert.IsNull(result);
        }
    }
}
