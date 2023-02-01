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
    public class ConfigurationRootSourceTests
    {
        [TestMethod]
        public void Get_Returns_Value_From_Root()
        {
            const string name = "Setting1";
            const string expected = "Value1";
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var source = new ConfigurationRootSource();

            var result = source.Get(property, out var value);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, value);
        }
        
        [TestMethod]
        public void Get_Returns_Value_From_SubSection()
        {
            const string name = "Setting3";
            const string expected = "Value3";
            
            var prefixAttribute = new ConfigurationRootPrefixAttribute("Section2");
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, new [] {prefixAttribute});

            var source = new ConfigurationRootSource();

            var result = source.Get(property, out var value);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, value);
        }
        
        [TestMethod]
        public void Get_Returns_Null_If_Section_Does_Not_Exist()
        {
            const string name = "wh4t4";
            
            var prefixAttribute = new ConfigurationRootPrefixAttribute("y25ej574q45h");

            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, new [] {prefixAttribute});

            var source = new ConfigurationRootSource();

            var result = source.Get(property, out var value);

            Assert.IsFalse(result);
            Assert.IsNull(value);
        }
    }
}
