using System;

namespace Configgy.Source
{
    /// <summary>
    /// A base class for any source attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class ValueSourceAttributeBase : Attribute, IValueSource
    {
        /// <inheritdoc cref="IValueSource.Get"/>
        public abstract bool Get(IConfigProperty property, out string value);
    }
}
