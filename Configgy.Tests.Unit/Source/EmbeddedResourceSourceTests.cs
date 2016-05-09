using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EmbeddedResourceSourceTests
    {
        [TestMethod]
        public void GetRawValue_Returns_Value_From_conf_Resource()
        {
            const string name = "TestValue1Embedded";
            const string value = "This is a string value.";

            var source = new EmbeddedResourceSource();

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Value_From_json_Resource()
        {
            const string name = "TestValue2Embedded";
            const string value = "[ \"string array\" ]";

            var source = new EmbeddedResourceSource();

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Value_From_xml_Resource()
        {
            const string name = "TestValue3Embedded";
            const string value = "<element>some xml</element>";

            var source = new EmbeddedResourceSource();

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Null_For_Resource_That_Doesnt_Exist()
        {
            const string name = "this resource doesn't exist";

            var source = new EmbeddedResourceSource();

            var result = source.GetRawValue(name, null);

            Assert.IsNull(result);
        }
    }
}
