using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Tests.NullableDisable;

namespace Configgy.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class NullableReferenceCheckerTests
    {
        [TestMethod]
        public void RequiresNullableReferenceCheck_Nulls_Allowed_Simple_Object_Requires_Check()
        {
            var checker = new NullableReferenceChecker();

            var propertyInfo = typeof(TestConfig)
                .GetProperty(nameof(TestConfig.Setting04))!;

            var property = new ConfigProperty("TestProperty", propertyInfo, null);

            var requiresCheck = checker.RequiresNullableReferenceCheck(property);
            
            Assert.IsTrue(requiresCheck);
        }
        
        [TestMethod]
        public void RequiresNullableReferenceCheck_Nulls_Not_Allowed_Simple_Object_Requires_Check()
        {
            var checker = new NullableReferenceChecker();

            var propertyInfo = typeof(TestConfig)
                .GetProperty(nameof(TestConfig.Setting01))!;

            var property = new ConfigProperty("TestProperty", propertyInfo, null);

            var requiresCheck = checker.RequiresNullableReferenceCheck(property);
            
            Assert.IsTrue(requiresCheck);
        }
        
        [TestMethod]
        public void RequiresNullableReferenceCheck_Nulls_Unknown_Simple_Object_No_Check()
        {
            var checker = new NullableReferenceChecker();

            var propertyInfo = typeof(NullableDisableTestConfig)
                .GetProperty(nameof(TestConfig.Setting01))!;

            var property = new ConfigProperty("TestProperty", propertyInfo, null);

            var requiresCheck = checker.RequiresNullableReferenceCheck(property);
            
            Assert.IsFalse(requiresCheck);
        }
        
        [TestMethod]
        public void RequiresNullableReferenceCheck_Value_Type_No_Check()
        {
            var checker = new NullableReferenceChecker();

            var propertyInfo = typeof(TestConfig)
                .GetProperty(nameof(TestConfig.Setting05))!;

            var property = new ConfigProperty("TestProperty", propertyInfo, null);

            var requiresCheck = checker.RequiresNullableReferenceCheck(property);
            
            Assert.IsFalse(requiresCheck);
        }
    }
}
