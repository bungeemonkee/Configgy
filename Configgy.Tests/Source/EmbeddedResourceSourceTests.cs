using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_json_Resource()
        {
            const string name = "TestValue2Embedded";
            const string expected = "[ \"string array\" ]";
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_xml_Resource()
        {
            const string name = "TestValue3Embedded";
            const string expected = "<element>some xml</element>";
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Allows_Names_With_Underscores()
        {
            const string name = "Test_Value_4_Embedded";
            const string expected = "Setting!";
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_For_Resources_That_Dont_Exist()
        {
            const string name = "NOT ACTUALLY A RESOURCE!!!!!!";
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, null);

            var source = new EmbeddedResourceSource();

            var result = source.Get(property, out var value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }
    }
}
