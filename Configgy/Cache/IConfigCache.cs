using System;

namespace Configgy.Cache
{
    /// <summary>
    /// Defines a config cache.
    /// </summary>
    public interface IConfigCache
    {
        /// <summary>
        /// Gets a value from the cache.
        /// Produces the value using the callback if no value exists.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="valueCallback">The callback to use to produce the value if it is not in the cache.</param>
        /// <returns></returns>
        object GetValue(string valueName, Func<string, object> valueCallback);

        /// <summary>
        /// Remove a value from the cache.
        /// </summary>
        /// <param name="valueName">The name of the value to remove from the cache.</param>
        void RemoveValue(string valueName);

        /// <summary>
        /// Remove all values from the cache.
        /// </summary>
        void Clear();
    }
}
