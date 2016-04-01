using System.Linq;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A value coercer that simply aggregates the results from multiple other value coercers.
    /// </summary>
    public class AggregateCoercer : IValueCoercer
    {
        private readonly IValueCoercer[] _coercers;

        /// <summary>
        /// Creates a default aggregate coercer that delegates to the following coercers:
        /// <list type="number">
        /// <item>Any coercers given as property attributes.</item>
        /// <item><see cref="GeneralCoercerAttribute"/></item>
        /// <item><see cref="TypeCoercerAttribute"/></item>
        /// </list>
        /// </summary>
        public AggregateCoercer()
            : this(new GeneralCoercerAttribute(), new TypeCoercerAttribute())
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

        /// <summary>
        /// Coerce the raw string value into the expected result type.
        /// </summary>
        /// <typeparam name="T">The expected result type after coercion.</typeparam>
        /// <param name="value">The raw string value to be coerced.</param>
        /// <param name="valueName">The name of the value to be coerced.</param>
        /// <param name="property">If this value is directly associated with a property on a <see cref="Config"/> instance this is the reference to that property.</param>
        /// <returns>The coerced value or null if the value could not be coerced.</returns>
        public object CoerceTo<T>(string value, string valueName, PropertyInfo property)
        {
            var propertyCoercers = property == null
                ? Enumerable.Empty<IValueCoercer>()
                : property
                .GetCustomAttributes(true)
                .OfType<IValueCoercer>();

            return propertyCoercers
                .Union(_coercers)
                .Select(c => c.CoerceTo<T>(value, valueName, property))
                .Where(r => r != null)
                .FirstOrDefault();
        }
    }
}
