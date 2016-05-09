using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    public class GeneralCoercerTests
    {
        [TestMethod]
        public void GeneralCoercer_CoerceTo_Works_With_Strings()
        {
            const string input = "some string";

            var coercer = new GeneralCoercerAttribute();

            var result = coercer.CoerceTo<string>(input, null, null);

            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void GeneralCoercer_CoerceTo_Works_With_Ints()
        {
            const string input = "243";
            const int expected = 243;

            var coercer = new GeneralCoercerAttribute();

            var result = coercer.CoerceTo<int>(input, null, null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GeneralCoercer_CoerceTo_Returns_Null_When_The_TypeConverter_Cant_Convert_From_String()
        {
            const string input = ".*";

            var coercer = new GeneralCoercerAttribute();

            var result = coercer.CoerceTo<Regex>(input, null, null);

            Assert.IsNull(result);
        }
    }
}
