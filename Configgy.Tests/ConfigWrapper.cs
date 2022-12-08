using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Transformation;
using Configgy.Validation;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConfigWrapper<T> : Config
    {
        public const string ThePropertyName = nameof(TheProperty);

        public ConfigWrapper()
            : base(new ConfigProvider())
        {
        }

        public ConfigWrapper(string[] commandLine)
            : base(new ConfigProvider(commandLine))
        {
        }

        public ConfigWrapper(IValueCache cache, IValueSource source, IValueTransformer transformer, IValueValidator validator, IValueCoercer coercer)
            : base(new ConfigProvider(cache, source, transformer, validator, coercer))
        {
        }

        public T TheProperty => Get<T>();

        public T Get_Wrapper(string valueName)
        {
            return Get<T>(valueName, nameof(TheProperty));
        }
    }
}