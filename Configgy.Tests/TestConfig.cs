using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Transformation;
using Configgy.Validation;

namespace Configgy.Tests
{
    public class TestConfig : Config
    {
        public string Setting01 => Get<string>();

        [ConfigurationRootPrefix("Section01")]
        public string Setting02 => Get<string>();

        [ConfigurationRootPrefix("Section02.Section03")]
        public string Setting03 => Get<string>();

        public TestConfig()
            : base(new DictionaryCache(), new ConfigurationRootSource(), new AggregateTransformer(), new AggregateValidator(), new AggregateCoercer())
        { }
    }
}
