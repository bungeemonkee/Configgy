using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    public class CommandLineSourceTests
    {
        [TestMethod]
        public void GetRawValue_Incudes_The_C_Switch()
        {
            const string name = "Testing";
            const string value = "Blah";
            var commandLine = new string[] { "/c:Testing=Blah" };

            var source = new CommandLineSource(commandLine);

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Incudes_The_Config_Switch()
        {
            const string name = "Testing";
            const string value = "Blah";
            var commandLine = new string[] { "/config:Testing=Blah" };

            var source = new CommandLineSource(commandLine);

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Ignores_Unknown_Switches()
        {
            const string name = "Testing";
            const string value = "Blah";
            var commandLine = new string[] { "/lol:Testing=Blah" };

            var source = new CommandLineSource(commandLine);

            var result = source.GetRawValue(name, null);

            Assert.IsNull(result);
        }
    }
}
