using System;
using System.Reflection;

namespace Configgy.Coercion
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class ValueCoercerAttributeBase : Attribute, IValueCoercer
    {
        public abstract object CoerceTo<T>(string value, string valueName, PropertyInfo property);
    }
}
