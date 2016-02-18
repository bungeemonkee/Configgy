using System;
using System.Collections.Generic;

namespace Configgy.Cache
{
    /// <summary>
    /// A simple dictionary-based config cache.
    /// </summary>
    public class DictionaryCache : IConfigCache
    {
        private IDictionary<string, object> _internal = new Dictionary<string, object>();

        public void Clear()
        {
            lock (this)
            {
                _internal.Clear();
            }
        }

        public object GetValue(string valueName, Func<string, object> valueCallback)
        {
            // get the value from the dictionary
            object result;
            if (_internal.TryGetValue(valueName, out result))
            {
                return result;
            }

            // lock so only one thread produces and ad the value
            lock(this)
            {
                // make sure no thread added the value while we waited for the lock
                if (_internal.TryGetValue(valueName, out result))
                {
                    return result;
                }

                // the value doesn't exist - produce it from the callback
                result = valueCallback(valueName);

                // put the value in the dictionary
                _internal.Add(valueName, result);
            }

            return result;
        }

        public void RemoveValue(string valueName)
        {
            if (!_internal.ContainsKey(valueName)) return;

            lock (this)
            {
                _internal.Remove(valueName);
            }
        }
    }
}
