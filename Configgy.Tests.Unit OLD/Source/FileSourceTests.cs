using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FileSourceTests
    {
        [TestMethod]
        public void Get_Returns_Value_From_conf_File()
        {
            const string name = "TestValue1";
            const string expected = "This is a string value.";

            var source = new FileSource();

            string value;
            var result = source.Get(name, null, out value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_json_File()
        {
            const string name = "TestValue2";
            const string expected = "[ \"string array\" ]";

            var source = new FileSource();

            string value;
            var result = source.Get(name, null, out value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_xml_File()
        {
            const string name = "TestValue3";
            const string expected = "<element>some xml</element>";

            var source = new FileSource();

            string value;
            var result = source.Get(name, null, out value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_For_File_That_Doesnt_Exist()
        {
            const string name = "this file doesn't exist";

            var source = new FileSource();

            string value;
            var result = source.Get(name, null, out value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }
    }
}
