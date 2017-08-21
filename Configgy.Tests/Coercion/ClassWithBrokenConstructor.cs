using System;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Coercion
{
    [ExcludeFromCodeCoverage]
    public class ClassWithBrokenConstructor
    {
        public ClassWithBrokenConstructor()
        {
            throw new Exception();
        }
    }
}