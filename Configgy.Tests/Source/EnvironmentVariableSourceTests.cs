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
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(Name, typeof(string), propertyMock.Object, null);

            var result = source.Get(property, out var value);

            Assert.AreEqual(Value, value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Returns_Null_When_No_Matching_Environment_Variable()
        {
            const string name = " garbage string: P(*TO(*HFJJJFS#@(**&&^$%#*&()FDGO*^FDC VBNJUYT";
            
            var propertyMock = new Mock<PropertyInfo>(MockBehavior.Strict);
            propertyMock.Setup(x => x.GetCustomAttributes(true))
                .Returns(Array.Empty<object>());
            propertyMock.SetupGet(x => x.Name)
                .Returns("property");
            
            IConfigProperty property = new ConfigProperty(name, typeof(string), propertyMock.Object, null);

            var source = new EnvironmentVariableSource();

            var result = source.Get(property, out var value);

            Assert.IsNull(value);
            Assert.IsFalse(result);
        }
    }
}