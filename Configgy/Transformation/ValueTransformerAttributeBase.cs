using System;

namespace Configgy.Transformation
{
    /// <summary>
    /// A base class for any transformer attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class ValueTransformerAttributeBase : Attribute, IValueTransformer
    {
        /// <summary>
        /// The order in which the transformers are applied.
        /// </summary>
        public virtual int Order { get; set; }

        /// <inheritdoc cref="IValueTransformer.Transform"/>
        public abstract string? Transform(IConfigProperty property, string? value);
    }
}
