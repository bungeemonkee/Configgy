using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    public class GeneralCoercerTests
    {
        [TestMethod]
        public void GeneralCoercer_CoerceTo_Works_With_Strings()
        {
            const string input = "some string";

            var coercer = new GeneralCoercer();

            var result = coercer.CoerceTo<string>(input, null, null);

            Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void GeneralCoercer_CoerceTo_Works_With_Ints()
        {
            const string input = "243";
            const int expected = 243;

            var coercer = new GeneralCoercer();

            var result = coercer.CoerceTo<int>(input, null, null);

            Assert.AreEqual(expected, result);
        }
    }
}
