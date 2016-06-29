using System;
using System.Reflection;

namespace Configgy.Transfomers
{
    /// <summary>
    /// A base class for any transformer attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class ValueTransformerAttributeBase : Attribute, IValueTransformer
    {
        public virtual int Order { get; set; }

        public abstract string Transform(string value, string valueName, PropertyInfo property);
    }
}
