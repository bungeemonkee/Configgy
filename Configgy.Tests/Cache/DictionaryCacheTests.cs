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
        public void Remove_Succeeds_For_Item_Not_In_The_Cache()
        {
            var cache = new DictionaryCache();

            cache.Remove("Not actually something that is in this cache.");
        }
    }
}