using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    public class FileSourceTests
    {
        [TestMethod]
        public void GetRawValue_Returns_Value_From_conf_File()
        {
            const string name = "TestValue1";
            const string value = "This is a string value.";

            var source = new FileSource();

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Value_From_json_File()
        {
            const string name = "TestValue2";
            const string value = "[ \"string array\" ]";

            var source = new FileSource();

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Value_From_xml_File()
        {
            const string name = "TestValue3";
            const string value = "<element>some xml</element>";

            var source = new FileSource();

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Null_For_File_That_Doesnt_Exist()
        {
            const string name = "this file doesn't exist";

            var source = new FileSource();

            var result = source.GetRawValue(name, null);

            Assert.IsNull(result);
        }
    }
}
