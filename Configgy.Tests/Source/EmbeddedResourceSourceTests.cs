using System.Diagnostics.CodeAnalysis;
using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EmbeddedResourceSourceTests
    {
        [TestMethod]
        public void Get_Returns_Value_From_conf_Resource()
        {
            const string name = "TestValue1Embedded";
            const string expected = "This is a string value.";
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out string value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_json_Resource()
        {
            const string name = "TestValue2Embedded";
            const string expected = "[ \"string array\" ]";
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out string value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_xml_Resource()
        {
            const string name = "TestValue3Embedded";
            const string expected = "<element>some xml</element>";
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out string value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Allows_Names_With_Underscores()
        {
            const string name = "Test_Value_4_Embedded";
            const string expected = "Setting!";
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out string value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_For_Resources_That_Dont_Exist()
        {
            const string name = "NOT ACTUALLY A RESOURCE!!!!!!";
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out var value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }
    }
}
