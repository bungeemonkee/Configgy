using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Cache;

namespace Configgy.Tests.Cache
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DictionaryCacheTests
    {
        [TestMethod]
        public void Add_Then_Get_Returns_Same_Value()
        {
            const string name = "name";
            const string value = "value";
            
            var cache = new DictionaryCache();

            cache.Add(name, value);

            var result = cache.Get(name, x => new Exception("Get() should not invoke callback!"));
            
            Assert.AreSame(value, result);
        }
        
        [TestMethod]
        public void Add_Twice_Then_Get_Returns_Second_Value()
        {
            const string name = "name";
            var value1 = new object();
            var value2 = new object();
            
            var cache = new DictionaryCache();

            cache.Add(name, value1);
            cache.Add(name, value2);

            var result = cache.Get(name, x => new Exception("Get() should not invoke callback!"));
            
            Assert.AreSame(value2, result);
        }
        
        [TestMethod]
        public void Remove_Succeeds_For_Item_Not_In_The_Cache()
        {
            var cache = new DictionaryCache();

            cache.Remove("Not actually something that is in this cache.");
        }
    }
}