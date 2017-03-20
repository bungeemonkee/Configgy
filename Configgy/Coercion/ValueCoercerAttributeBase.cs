using System;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A base class for any coercer attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class ValueCoercerAttributeBase : Attribute, IValueCoercer
    {
        private static readonly Type NullableType = typeof(Nullable<>);

        /// <summary>
        /// Coerce the raw string value into the expected result type.
        /// </summary>
        /// <typeparam name="T">The expected result type after coercion.</typeparam>
        /// <param name="value">The raw string value to be coerced.</param>
        /// <param name="valueName">The name of the value to be coerced.</param>
        /// <param name="property">If this value is directly associated with a property on a <see cref="Config"/> instance this is the reference to that property.</param>
        /// <param name="result">The coerced value.</param>
        /// <returns>True if the value could be coerced, false otherwise.</returns>
        public abstract bool Coerce<T>(string value, string valueName, ICustomAttributeProvider property, out T result);

        protected static bool IsNullable<T>()
        {
            var type = typeof(T);
            return type.GetTypeInfo().IsClass || (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == NullableType);
        }
    }
}
