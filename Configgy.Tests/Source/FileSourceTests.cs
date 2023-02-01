using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Source;
using Moq;

namespace Configgy.Tests.Source
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
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var source = new FileSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_json_File()
        {
            const string name = "TestValue2";
            const string expected = "[\"string array\"]";
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var source = new FileSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Value_From_xml_File()
        {
            const string name = "TestValue3";
            const string expected = "<element>some xml</element>";
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var source = new FileSource();

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_For_File_That_Doesnt_Exist()
        {
            const string name = "this file doesn't exist";
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var source = new FileSource();

            var result = source.Get(property, out var value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }
    }
}