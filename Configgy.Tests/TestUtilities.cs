using System.Reflection;

namespace Configgy.Tests;

public static class TestUtilities
{
    public static readonly PropertyInfo NonNullableProperty =
        typeof(TestConfig).GetProperty(nameof(TestConfig.Setting01))!;
    
    public static readonly PropertyInfo NullableProperty =
        typeof(TestConfig).GetProperty(nameof(TestConfig.Setting04))!;

    public static readonly PropertyInfo NonNullableBoolean =
        typeof(TestConfig).GetProperty(nameof(TestConfig.Setting06))!;
}