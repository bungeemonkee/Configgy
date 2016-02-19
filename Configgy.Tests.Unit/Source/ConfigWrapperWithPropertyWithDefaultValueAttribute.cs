using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Validation;
using System.ComponentModel;

namespace Configgy.Tests.Unit
{
    public class ConfigWrapperWithPropertyWithefaultValueAttribute<T> : Config
    {
        public const string ThePropertyName = nameof(TheProperty);

        [DefaultValue("1")]
        public T TheProperty { get { return Get<T>(); } }

        public ConfigWrapperWithPropertyWithefaultValueAttribute(IValueCache cache, IValueSource source, IValueValidator validator, IValueCoercer coercer)
            : base(cache, source, validator, coercer)
        {
        }

        public T Get_Wrapper(string valueName)
        {
            return Get<T>(valueName);
        }
    }
}
