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

        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>
        public abstract bool Coerce<T>(IConfigProperty property, string? value, out T result);

        /// <summary>
        /// Determine if the type T is nullable or not. 
        /// </summary>
        /// <typeparam name="T">The type </typeparam>
        /// <returns>True if the type is nullable, false otherwise.</returns>
        protected static bool IsNullable<T>()
        {
            // TODO: This probably needs to be updated to resolve issue #35
            // https://github.com/bungeemonkee/Configgy/issues/35
            var type = typeof(T);
            return type.GetTypeInfo().IsClass || (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == NullableType);
        }
    }
}
