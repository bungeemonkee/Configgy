using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    public class JsonCoercerTests
    {
        [TestMethod]
        public void JsonCoercer_CoerceTo_Works_With_Array_Of_Int()
        {
            const string input = "[1,4,78,222]";
            var expected = new int[] { 1, 4, 78, 222 };

            var coercer = new JsonCoercer();

            var result = coercer.CoerceTo<int[]>(input, null, null) as int[];

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void JsonCoercer_CoerceTo_Works_With_Dictionary_Of_String_String()
        {
            const string input = "{'Banana':'Good','Apple':'Yummy','Radish':'Icky'}";
            var expected = new Dictionary<string, string>
            {
                ["Banana"] = "Good",
                ["Apple"] = "Yummy",
                ["Radish"] = "Icky"
            };

            var coercer = new JsonCoercer();

            var result = coercer.CoerceTo<Dictionary<string, string>>(input, null, null) as IDictionary;

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void JsonCoercer_CoerceTo_Returns_Null_With_Invalid_Json()
        {
            const string input = "{";

            var coercer = new JsonCoercer();

            var result = coercer.CoerceTo<Dictionary<string, string>>(input, null, null);

            Assert.IsNull(result);
        }
    }
}
