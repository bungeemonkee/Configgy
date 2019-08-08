using Configgy.Source;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Source
{
    [ExcludeFromCodeCoverage]
    public class ValueSourceStub : IValueSource
    {
        public bool Get(IConfigProperty property, out string value)
        {
            throw new NotImplementedException();
        }
    }
}