using Configgy.Source;

namespace Configgy.Tests
{
    public class TestConfig : Config
    {
        public string Setting01 => Get<string>();

        [ConfigurationRootPrefix("Section01")]
        public string Setting02 => Get<string>();

        [ConfigurationRootPrefix("Section02.Section03")]
        public string Setting03 => Get<string>();

        public object? Setting04 => Get<object?>();

        public int Setting05 => Get<int>();

        public bool Setting06 => Get<bool>();

        public TestConfig()
            : base(new ConfigProvider())
        {
        }
    }
}
