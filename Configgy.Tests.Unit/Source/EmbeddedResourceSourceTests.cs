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
        public void GetRawValue_Allows_Names_With_Underscores()
        {
            const string name = "Test_Value_4_Embedded";
            const string value = "Setting!";

            var source = new EmbeddedResourceSource();

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }
    }
}
