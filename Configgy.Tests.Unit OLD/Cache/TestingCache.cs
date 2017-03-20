using Configgy.Cache;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Cache
{
    /// <summary>
    /// A testing cache implementation that doesn't actually cache anything
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TestingCache : IValueCache
    {
        public void Clear()
        {
            // Nothing to do
        }

        public object Get(string valueName, Func<string, object> valueCallback)
        {
            return valueCallback(valueName);
        }

        public void Remove(string valueName)
        {
            // Nothing to do
        }
    }
}
