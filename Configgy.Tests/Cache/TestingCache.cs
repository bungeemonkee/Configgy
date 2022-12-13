using Configgy.Cache;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Cache
{
    /// <summary>
    ///     A testing cache implementation that doesn't actually cache anything
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TestingCache : IValueCache
    {
        public void Add(string valueName, object? value)
        {
            // Nothing to do
        }

        public object? Get(string valueName, Func<string, object?> valueCallback)
        {
            return valueCallback(valueName);
        }

        public void Remove()
        {
            // Nothing to do
        }

        public void Remove(string valueName)
        {
            // Nothing to do
        }
    }
}