using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Transfomers;
using Configgy.Validation;

namespace Configgy.Tests.Unit
{
    public class ConfigWrapperWithNoProperties : Config
    {
        public ConfigWrapperWithNoProperties(IValueCache cache, IValueSource source, IValueTransformer transformer, IValueValidator validator, IValueCoercer coercer)
            : base(cache, source, transformer, validator, coercer)
        {
        }

        public T Get_Wrapper<T>(string valueName)
        {
            return Get<T>(valueName);
        }
    }
}
