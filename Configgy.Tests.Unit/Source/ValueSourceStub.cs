using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Configgy.Source;

namespace Configgy.Tests.Unit.Source
{
    [ExcludeFromCodeCoverage]
    public class ValueSourceStub : IValueSource
    {
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            throw new NotImplementedException();
        }
    }
}
