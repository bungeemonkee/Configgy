using System;

namespace Configgy.Source
{
    /// <summary>
    /// Prevents a specific source from being used to retrieve a given configuration property.
    /// </summary>
    /// <remarks>
    /// This attribute is respected by <see cref="AggregateSource"/>.
    /// <see cref="AggregateSource"/> is the default source implementation, but if it is overridden this property may not be effective.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class PreventSourceAttribute : Attribute
    {
        /// <summary>
        /// The type of the source to ignore.
        /// This source will be ignored by <see cref="AggregateSource"/> for this property.
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// Prevent a specific source from being used for this property.
        /// </summary>
        /// <param name="type">The source type to ignore. See <see cref="AggregateSource"/>.</param>
        public PreventSourceAttribute(Type type)
        {
            SourceType = type;
        }
    }
}
