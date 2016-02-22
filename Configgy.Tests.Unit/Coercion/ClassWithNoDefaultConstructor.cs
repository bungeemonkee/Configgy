
namespace Configgy.Tests.Unit.Coercion
{
    public class ClassWithNoDefaultConstructor
    {
        public readonly int Value;

        public ClassWithNoDefaultConstructor(int value)
        {
            Value = value;
        }
    }
}
