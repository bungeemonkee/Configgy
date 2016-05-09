using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EnvironmentVariableSourceTests
    {
        public const string Name = "environment variable testing name";
        public const string Value = "environment variable testing value";

        [TestInitialize]
        public void Begin()
        {
            Environment.SetEnvironmentVariable(Name, Value, EnvironmentVariableTarget.Process);
        }

        [TestCleanup]
        public void End()
        {
            Environment.SetEnvironmentVariable(Name, null, EnvironmentVariableTarget.Process);
        }

        [TestMethod]
        public void GetRawValue_Returns_Value_From_Environment()
        {

            var source = new EnvironmentVariableSource();

            var result = source.GetRawValue(Name, null);

            Assert.AreEqual(Value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Null_When_No_Matching_Environment_Variable()
        {
            var name = Name + " garbage string: P(*TO(*HFJJJFS#@(**&&^$%#*&()FDGO*^FDC VBNJUYT";

            var source = new EnvironmentVariableSource();

            var result = source.GetRawValue(name, null);

            Assert.IsNull(result);
        }
    }
}
