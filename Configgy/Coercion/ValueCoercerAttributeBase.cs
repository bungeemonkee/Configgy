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
        public abstract bool Coerce<T>(IConfigProperty property, string value, out T result);

        protected static bool IsNullable<T>()
        {
            var type = typeof(T);
            return type.GetTypeInfo().IsClass || (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == NullableType);
        }
    }
}
