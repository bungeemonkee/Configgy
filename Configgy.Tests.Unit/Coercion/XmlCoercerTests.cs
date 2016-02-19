using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    public class XmlCoercerTests
    {
        [TestMethod]
        public void XmlCoercer_CoerceTo_Works_With_Array_Of_Int()
        {
            const string input =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfInt xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <int>1</int>
    <int>4</int>
    <int>78</int>
    <int>222</int>
</ArrayOfInt>";

            var expected = new int[] { 1, 4, 78, 222 };

            var coercer = new XmlCoercerAttribute();

            var result = coercer.CoerceTo<int[]>(input, null, null) as ICollection;

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void XmlCoercer_CoerceTo_Returns_Null_With_Invalid_Xml()
        {
            const string input =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfInt xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <int>1</int
    <int>4</int>
    <int>78</int>
    <int>222</int>
</ArrayOfInt>";

            var coercer = new XmlCoercerAttribute();

            var result = coercer.CoerceTo<int[]>(input, null, null);

            Assert.IsNull(result);
        }
    }
}
