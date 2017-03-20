using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Configgy.Source;

namespace Configgy.Tests.Unit.Source
{
    [ExcludeFromCodeCoverage]
    public class ValueSourceStub : IValueSource
    {
        public bool Get(string valueName, PropertyInfo property, out string value)
        {
            throw new NotImplementedException();
        }
    }
}
