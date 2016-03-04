using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    public class CsvCoercerTests
    {
        [TestMethod]
        public void CoerceTo_Works_With_Array_Of_Int()
        {
            const string input = "1,4,78,222";
            var expected = new int[] { 1, 4, 78, 222 };

            var coercer = new CsvCoercerAttribute(typeof(int));

            var result = coercer.CoerceTo<int[]>(input, null, null) as int[];

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CoerceTo_Works_With_Custom_Separator()
        {
            const string input = "1 | 4 | 78 | 222";
            var expected = new int[] { 1, 4, 78, 222 };

            var coercer = new CsvCoercerAttribute(typeof(int), " | ");

            var result = coercer.CoerceTo<int[]>(input, null, null) as int[];

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_With_Invalid_Type()
        {
            const string input = "1,4,78,222";

            var coercer = new CsvCoercerAttribute(typeof(int));

            var result = coercer.CoerceTo<long[]>(input, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_With_Invalid_Values()
        {
            const string input = "1,4,78,abcdefg";

            var coercer = new CsvCoercerAttribute(typeof(int));

            var result = coercer.CoerceTo<int[]>(input, null, null);

            Assert.IsNull(result);
        }
    }
}
