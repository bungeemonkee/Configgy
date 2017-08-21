using Configgy.Source;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Configgy.Tests.Source
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