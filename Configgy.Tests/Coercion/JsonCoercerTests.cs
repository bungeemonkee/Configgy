using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Coercion;

namespace Configgy.Tests.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class JsonCoercerTests
    {
        [TestMethod]
        public void JsonCoercer_Coerce_Works_With_Array_Of_Int()
        {
            const string input = "[1,4,78,222]";
            var expected = new[] {1, 4, 78, 222};

            var coercer = new JsonCoercerAttribute();

            var coerced = coercer.Coerce(null, input, out int[] result);

            CollectionAssert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void JsonCoercer_Coerce_Works_With_Dictionary_Of_String_String()
        {
            const string input = "{\"Banana\":\"Good\",\"Apple\":\"Yummy\",\"Radish\":\"Icky\"}";
            var expected = new Dictionary<string, string>
            {
                ["Banana"] = "Good",
                ["Apple"] = "Yummy",
                ["Radish"] = "Icky"
            };

            var coercer = new JsonCoercerAttribute();

            var coerced = coercer.Coerce(null, input, out Dictionary<string, string> result);

            CollectionAssert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void JsonCoercer_Coerce_Throws_Exception_With_Invalid_Json()
        {
            const string input = "{";

            var coercer = new JsonCoercerAttribute();

            var coerced = coercer.Coerce(null, input, out Dictionary<string, string> result);

            Assert.IsNull(result);
            Assert.IsFalse(coerced);
        }

        [TestMethod]
        public void JsonCoercer_Coerce_Returns_Null_With_Null_Json()
        {
            const string input = null;

            var coercer = new JsonCoercerAttribute();

            var coerced = coercer.Coerce(null, input, out Dictionary<string, string> result);

            Assert.IsNull(result);
            Assert.IsTrue(coerced);
        }
    }
}