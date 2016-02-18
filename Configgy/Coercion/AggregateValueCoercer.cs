using System.Linq;
using System.Reflection;

namespace Configgy.Coercion
{
    /// <summary>
    /// A value coercer that simply aggregates the results from multiple other value coercers.
    /// </summary>
    public class AggregateValueCoercer : IValueCoercer
    {
        private readonly IValueCoercer[] _coercers;

        public AggregateValueCoercer()
        {
            // TODO: Create the default coercer set
        }

        public AggregateValueCoercer(params IValueCoercer[] coercers)
        {
            _coercers = coercers;
        }

        public object CoerceTo<T>(string value, string valueName, PropertyInfo property)
        {
            return _coercers
                .Select(c => c.CoerceTo<T>(value, valueName, property))
                .Where(r => r != null)
                .FirstOrDefault();
        }
    }
}
