using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Source;

namespace Configgy.Tests.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DashedCommandLineSourceTests
    {
        [TestMethod]
        public void Get_Includes_Defined_Values()
        {
            const string name = "Testing";
            const string expected = "Blah";
            var commandLine = new[] {"--Testing=Blah"};
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Assumes_True_For_Boolean_With_No_Value()
        {
            const string name = "Testing";
            const string expected = "True";
            var commandLine = new[] {"--Testing"};
            
            IConfigProperty property = new ConfigProperty(name, typeof(bool), null, null);

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_For_Non_Boolean_With_No_Value()
        {
            const string name = "Testing";
            var commandLine = new[] {"--Testing"};
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);
            
            Assert.IsNull(value);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_Ignores_Unknown_Options()
        {
            const string name = "Testing";
            var commandLine = new[] {"--Banana=Blah"};
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_Is_Not_Case_Sensitive()
        {
            const string name = "Testing";
            const string expected = "Blah";
            var commandLine = new[] {"--tEstinG=Blah"};
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, null);

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Uses_Name_Override_From_CommandLineNameAttribute()
        {
            const string name = "Testing";
            const string nameOverride = "test";
            const string expected = "1234";
            var commandLine = new[] {"--test=1234"};

            var attribute = new CommandLineNameAttribute(nameOverride);
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), null, new [] {attribute});

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, value);
        }
    }
}