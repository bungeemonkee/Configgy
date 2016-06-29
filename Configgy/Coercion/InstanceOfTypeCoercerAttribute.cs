using System;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A coercer that creates instances of the given type name.
    /// </summary>
    /// <remarks>
    /// See the documentation for <see cref="Type.GetType(string, bool, bool)"/> and <see cref="Activator.CreateInstance(Type)"/>.
    /// </remarks>
    public class InstanceOfTypeCoercerAttribute : ValueCoercerAttributeBase, IValueCoercer
    {
        /// <summary>
        /// Coerce the raw string value into the expected result type.
        /// </summary>
        /// <typeparam name="T">The expected result type after coercion.</typeparam>
        /// <param name="value">The raw string value to be coerced.</param>
        /// <param name="valueName">The name of the value to be coerced.</param>
        /// <param name="property">If this value is directly associated with a property on a <see cref="Config"/> instance this is the reference to that property.</param>
        /// <param name="result">The coerced value.</param>
        /// <returns>True if the value could be coerced, false otherwise.</returns>
        public override bool Coerce<T>(string value, string valueName, PropertyInfo property, out T result)
        {
            var type = Type.GetType(value, false, true);
            if (type == null)
            {
                result = default(T);
                return false;
            }

            result = (T)Activator.CreateInstance(type);
            return true;
        }
    }
}
