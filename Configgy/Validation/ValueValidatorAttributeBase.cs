using System;
using System.Reflection;

namespace Configgy.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class ValueValidatorAtributeBase : Attribute, IValueValidator
    {
        public abstract void Validate<T>(string value, string valueName, PropertyInfo property);
    }
}
