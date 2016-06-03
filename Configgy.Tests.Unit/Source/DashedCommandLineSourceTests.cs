using Configgy.Source;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DashedCommandLineSourceTests
    {
        [TestMethod]
        public void GetRawValue_Incudes_Defined_Values()
        {
            const string name = "Testing";
            const string value = "Blah";
            var commandLine = new string[] { "--Testing=Blah" };

            var source = new DashedCommandLineSource(commandLine);

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Assumes_True_For_Booean_With_No_Value()
        {
            const string name = "Testing";
            const string value = "True";
            var commandLine = new string[] { "--Testing" };

            var propertyMock = new Mock<PropertyInfo>();
            propertyMock.SetupGet(x => x.PropertyType).Returns(typeof(bool));

            var source = new DashedCommandLineSource(commandLine);

            var result = source.GetRawValue(name, propertyMock.Object);

            Assert.AreEqual(value, result);
            propertyMock.VerifyGet(x => x.PropertyType);
        }

        [TestMethod]
        public void GetRawValue_Returns_Null_For_Non_Booean_With_No_Value()
        {
            const string name = "Testing";
            var commandLine = new string[] { "--Testing" };

            var propertyMock = new Mock<PropertyInfo>();
            propertyMock.SetupGet(x => x.PropertyType).Returns(typeof(string));

            var source = new DashedCommandLineSource(commandLine);

            var result = source.GetRawValue(name, propertyMock.Object);

            Assert.IsNull(result);
            propertyMock.VerifyGet(x => x.PropertyType);
        }

        [TestMethod]
        public void GetRawValue_Ignores_Unknown_Options()
        {
            const string name = "Testing";
            var commandLine = new string[] { "--Banana=Blah" };

            var source = new DashedCommandLineSource(commandLine);

            var result = source.GetRawValue(name, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRawValue_Is_Not_Case_Sensitive()
        {
            const string name = "Testing";
            const string expected = "Blah";
            var commandLine = new string[] { "--tEstinG=Blah" };

            var source = new DashedCommandLineSource(commandLine);

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(expected, result);
        }
    }
}
