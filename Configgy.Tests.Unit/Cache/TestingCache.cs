using Configgy.Cache;
using System;

namespace Configgy.Tests.Unit.Cache
{
    /// <summary>
    /// A testing cache implementation that doesn't actually cache anything
    /// </summary>
    public class TestingCache : IValueCache
    {
        public void Clear()
        {
            // Nothing to do
        }

        public object GetValue(string valueName, Func<string, object> valueCallback)
        {
            return valueCallback(valueName);
        }

        public void RemoveValue(string valueName)
        {
            // Nothing to do
        }
    }
}
