using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Validation;

namespace Configgy.Tests.Unit
{
    public class ConfigWrapperWithPropertyWithCoercerProperty<T> : Config
    {
        public const string ThePropertyName = nameof(TheProperty);

        [JsonCoercerAttribute]
        public T TheProperty { get { return Get<T>(); } }

        public ConfigWrapperWithPropertyWithCoercerProperty(IValueCache cache, IValueSource source, IValueValidator validator, IValueCoercer coercer)
            : base(cache, source, validator, coercer)
        {
        }

        public T Get_Wrapper(string valueName)
        {
            return Get<T>(valueName);
        }
    }
}
