using System.Diagnostics.CodeAnalysis;
using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigPropertyTests
    {

        [TestMethod]
        public void ValueName_Comes_From_AlternateNameAttribute()
        {
            const string name = "Testing";
            const string nameOverride = "test";

            var attribute = new AlternateNameAttribute(nameOverride);
            
            var property = new ConfigProperty(name, typeof(string), null, new [] {attribute});

            Assert.AreEqual(nameOverride, property.ValueName);
        }
        
    }
}
