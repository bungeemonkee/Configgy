using System.Collections.Generic;
using System.Linq;

namespace Configgy.Coercion
{
    /// <summary>
    /// A value coercer that simply aggregates the results from multiple other value coercers.
    /// </summary>
    public class AggregateCoercer : IValueCoercer
    {
        private readonly IValueCoercer[] _coercers;

        /// <summary>
        /// The <see cref="IValueCoercer"/>s used.
        /// This does not include ones defined as property attributes.
        /// </summary>
        public IEnumerable<IValueCoercer> Coercers => _coercers;

        /// <summary>
        /// Creates a default aggregate coercer that delegates to the following coercers:
        /// <list type="number">
        /// <item>Any coercers given as property attributes.</item>
        /// <item><see cref="GeneralCoercerAttribute"/></item>
        /// <item><see cref="TypeCoercerAttribute"/></item>
        /// </list>
        /// </summary>
        public AggregateCoercer()
            : this(new RegexCoercerAttribute(), new TypeCoercerAttribute(), new GeneralCoercerAttribute())
        {
        }

        /// <summary>
        /// Creates an aggregate coercer that delegates to any coercers as property attributes then the given coercers.
        /// </summary>
        /// <param name="coercers">The coerces this aggregate coercer will delegate to.</param>
        public AggregateCoercer(params IValueCoercer[] coercers)
        {
            _coercers = coercers;
        }

        
        /// <inheritdoc cref="IValueCoercer.Coerce{T}"/>
        public bool Coerce<T>(IConfigProperty property, string value, out T result)
        {
            var propertyCoercers = property.Attributes
                .OfType<IValueCoercer>();

            foreach (var coercer in propertyCoercers.Concat(_coercers))
            {
                if (coercer.Coerce(property, value, out result)) return true;
            }

            result = default;
            return false;
        }
    }
}
