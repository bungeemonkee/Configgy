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

        public AggregateCoercer()
            : this(new GeneralCoercerAttribute(), new TypeCoercerAttribute())
        {
        }

        public AggregateCoercer(params IValueCoercer[] coercers)
        {
            _coercers = coercers;
        }

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
