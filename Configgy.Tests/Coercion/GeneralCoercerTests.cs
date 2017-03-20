using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class GeneralCoercerTests
    {
        [TestMethod]
        public void GeneralCoercer_Coerce_Works_With_Strings()
        {
            const string input = "some string";

            var coercer = new GeneralCoercerAttribute();

            string result;
            var coerced = coercer.Coerce(input, null, null, out result);

            Assert.AreEqual(input, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void GeneralCoercer_Coerce_Works_With_Ints()
        {
            const string input = "243";
            const int expected = 243;

            var coercer = new GeneralCoercerAttribute();

            int result;
            var coerced = coercer.Coerce<int>(input, null, null, out result);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void GeneralCoercer_Coerce_Returns_Null_When_The_TypeConverter_Cant_Convert_From_String()
        {
            const string input = ".*";

            var coercer = new GeneralCoercerAttribute();

            Regex result;
            var coerced = coercer.Coerce(input, null, null, out result);

            Assert.IsNull(result);
            Assert.IsFalse(coerced);
        }

        [TestMethod]
        public void GeneralCoercer_Coerce_Returns_Null_For_Null_String_When_Type_Is_Nullable()
        {
            var coercer = new GeneralCoercerAttribute();

            int? result;
            var coerced = coercer.Coerce(null, null, null, out result);

            Assert.IsNull(result);
            Assert.IsTrue(coerced);
        }
    }
}
