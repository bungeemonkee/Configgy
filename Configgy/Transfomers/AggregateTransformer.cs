using System.Linq;
using System.Reflection;

namespace Configgy.Transfomers
{
    /// <summary>
    /// A value transformer that applies multiple other value transformers.
    /// </summary>
    public class AggregateTransformer : IValueTransformer
    {
        private readonly IValueTransformer[] _transformers;

        /// <summary>
        /// A simple ordering mechanism used to ensure transformers are chained in the correct sequence.
        /// For AggregateTransformer this value is always 10.
        /// </summary>
        public int Order => 10;

        /// <summary>
        /// Creates a default aggregate transformer that only looks for other transformers as property attributes.
        /// </summary>
        public AggregateTransformer()
            : this(new IValueTransformer[0])
        { }

        /// <summary>
        /// Creates an aggregate transformer that delegates to any transformers as property attributes followed by the given transformers.
        /// </summary>
        /// <param name="transformers">The transformers this aggregate transformer will delegate to.</param>
        public AggregateTransformer(params IValueTransformer[] transformers)
        {
            _transformers = transformers;
        }

        public string TransformValue(string value, string valueName, PropertyInfo property)
        {
            var propertyTransformers = property == null
                ? Enumerable.Empty<IValueTransformer>()
                : property
                    .GetCustomAttributes(true)
                    .OfType<IValueTransformer>();

            return propertyTransformers
                .Union(_transformers)
                .OrderBy(x => x.Order)
                .Aggregate(value, (x, y) => y.TransformValue(x, valueName, property));
        }
    }
}
