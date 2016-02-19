using Configgy.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Cache
{
    [TestClass]
    public class DictionaryCacheTests
    {
        [TestMethod]
        public void Remove_Succeeds_For_Item_Not_In_The_Cache()
        {
            var cache = new DictionaryCache();

            cache.RemoveValue("Not actually something that is in this cache.");
        }
    }
}
