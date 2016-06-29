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
        public void Get_Incudes_Defined_Values()
        {
            const string name = "Testing";
            const string expected = "Blah";
            var commandLine = new [] { "--Testing=Blah" };

            var source = new DashedCommandLineSource(commandLine);

            string value;
            var result = source.Get(name, null, out value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Assumes_True_For_Booean_With_No_Value()
        {
            const string name = "Testing";
            const string expected = "True";
            var commandLine = new [] { "--Testing" };

            var propertyMock = new Mock<PropertyInfo>();
            propertyMock.SetupGet(x => x.PropertyType).Returns(typeof(bool));

            var source = new DashedCommandLineSource(commandLine);

            string value;
            var result = source.Get(name, propertyMock.Object, out value);

            propertyMock.VerifyGet(x => x.PropertyType);
            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_For_Non_Booean_With_No_Value()
        {
            const string name = "Testing";
            var commandLine = new [] { "--Testing" };

            var propertyMock = new Mock<PropertyInfo>();
            propertyMock.SetupGet(x => x.PropertyType).Returns(typeof(string));

            var source = new DashedCommandLineSource(commandLine);

            string value;
            var result = source.Get(name, propertyMock.Object, out value);

            propertyMock.VerifyGet(x => x.PropertyType);
            Assert.IsNull(value);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_Ignores_Unknown_Options()
        {
            const string name = "Testing";
            var commandLine = new [] { "--Banana=Blah" };

            var source = new DashedCommandLineSource(commandLine);

            string value;
            var result = source.Get(name, null, out value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_Is_Not_Case_Sensitive()
        {
            const string name = "Testing";
            const string expected = "Blah";
            var commandLine = new [] { "--tEstinG=Blah" };

            var source = new DashedCommandLineSource(commandLine);

            string value;
            var result = source.Get(name, null, out value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }
    }
}
