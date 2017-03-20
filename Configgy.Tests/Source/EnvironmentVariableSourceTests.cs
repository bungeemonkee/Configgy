using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class EnvironmentVariableSourceTests
    {
        public const string Name = "environment variable testing name";
        public const string Value = "environment variable testing value";

        [TestInitialize]
        public void Begin()
        {
            Environment.SetEnvironmentVariable(Name, Value);
        }

        [TestCleanup]
        public void End()
        {
            Environment.SetEnvironmentVariable(Name, null);
        }

        [TestMethod]
        public void Get_Returns_Value_From_Environment()
        {
            var source = new EnvironmentVariableSource();

            var result = source.Get(Name, null, out string value);

            Assert.AreEqual(Value, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_When_No_Matching_Environment_Variable()
        {
            const string name = " garbage string: P(*TO(*HFJJJFS#@(**&&^$%#*&()FDGO*^FDC VBNJUYT";

            var source = new EnvironmentVariableSource();

            var result = source.Get(name, null, out string value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }
    }
}
