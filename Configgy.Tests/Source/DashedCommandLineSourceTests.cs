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
    public class DashedCommandLineSourceTests
    {
        [TestMethod]
        public void Get_Includes_Defined_Values()
        {
            const string name = "Testing";
            const string expected = "Blah";
            var commandLine = new[] {"--Testing=Blah"};
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Assumes_True_For_Boolean_With_No_Value()
        {
            var name = TestUtilities.NonNullableBoolean.Name;
            const string expected = "True";
            var commandLine = new[] {"--" + name};
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NonNullableBoolean, null);

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_For_Non_Boolean_With_No_Value()
        {
            var name = TestUtilities.NullableProperty.Name;
            var commandLine = new[] {"--" + name};
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

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
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

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
            
            IConfigProperty property = new ConfigProperty(name, TestUtilities.NullableProperty, null);

            var source = new DashedCommandLineSource(commandLine);

            var result = source.Get(property, out var value);

            Assert.AreEqual(expected, value);
            Assert.IsTrue(result);
        }
    }
}