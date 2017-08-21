using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Coercion
{
    [ExcludeFromCodeCoverage]
    public class ClassWithNoDefaultConstructor
    {
        public readonly int Value;

        public ClassWithNoDefaultConstructor(int value)
        {
            Value = value;
        }
    }
}