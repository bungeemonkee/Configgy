using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class XmlCoercerTests
    {
        [TestMethod]
        public void XmlCoercer_CoerceTo_Works_With_Array_Of_Int()
        {
            const string input = @"
<ArrayOfint xmlns=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
    <int>1</int>
    <int>4</int>
    <int>78</int>
    <int>222</int>
</ArrayOfint>
";

            var expected = new int[] { 1, 4, 78, 222 };

            var coercer = new XmlCoercerAttribute();

            var result = coercer.CoerceTo<int[]>(input, null, null) as ICollection;

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void XmlCoercer_CoerceTo_Returns_Null_With_Invalid_Xml()
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

            var result = coercer.CoerceTo<int[]>(input, null, null);

            Assert.IsNull(result);
        }
    }
}
