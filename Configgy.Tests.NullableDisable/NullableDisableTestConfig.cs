using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.NullableDisable;

[ExcludeFromCodeCoverage]
public class NullableDisableTestConfig : Config
{
    public object Setting01 => Get<object>();

    public NullableDisableTestConfig()
        : base (new ConfigProvider())
    {
    }
}