using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConnectionStringSourceTests
    {
        [TestMethod]
        public void Get_Returns_String_From_ConnectionStrings()
        {
            const string name = "name";
            const string expected = "value";

            var source = new ConectionStringsSource();

            string value;
            var result = source.Get(name, null, out value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_When_No_Matching_ConnectionStrings_Value()
        {
            const string name = "kfnnpa";

            var source = new ConectionStringsSource();

            string value;
            var result = source.Get(name, null, out value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }
    }
}
