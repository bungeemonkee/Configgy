using System.Collections.Generic;
using Configgy.Coercion;
using Configgy.Source;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests
{
    [ExcludeFromCodeCoverage]
    [Help("A program that does things. It's so good. Guys, you have no idea. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.")]
    public class ConfigWrapperWithHelpAttributes : Config
    {
        public ConfigWrapperWithHelpAttributes()
            : base(new ConfigProvider())
        {
        }
        
        [Help("This is a number.")]
        public int SomeNumber => Get<int>();

        [Help("")]
        public bool JulieDoTheThing => Get<bool>();

        public bool AConfigurationFlag => Get<bool>();

        [Help("This is going to be very long help text. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.")]
        public string TextualizedParameter => Get<string>();

        [Help("A comma-delimited list of bytes. VeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongWord.")]
        [CsvCoercer(typeof(byte))]
        public byte[] SomeBytes => Get<byte[]>();

        [Help("A really pretty unlikely value.")]
        public ushort? NullableUshort => Get<ushort?>();

        [XmlCoercer]
        [PreventSource(typeof(DashedCommandLineSource))]
        public byte[] XmlThatShouldNotBeIncludedEver => Get<byte[]>();

        [Help("Some enum value that means something.")]
        public TestingEnum EnumSetting => Get<TestingEnum>();

        public IList<decimal> Decimals => Get<IList<decimal>>();
    }
}