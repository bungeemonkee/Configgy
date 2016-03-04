using System;
using System.Collections.Concurrent;

namespace Configgy.Cache
{
    /// <summary>
    /// A simple dictionary-based config cache.
    /// </summary>
    public class DictionaryCache : IValueCache
    {
        private ConcurrentDictionary<string, object> _internal = new ConcurrentDictionary<string, object>();

        public void Clear()
        {
            _internal.Clear();
        }

        public object GetValue(string valueName, Func<string, object> valueCallback)
        {
            return _internal.GetOrAdd(valueName, valueCallback);
        }

        public void RemoveValue(string valueName)
        {
            object value;
            _internal.TryRemove(valueName, out value);
        }
    }
}
