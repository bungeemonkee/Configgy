using System;

namespace Configgy
{
    public interface IConfigProperty
    {
        string ValueName { get; }
        
        string? PropertyName { get; }

        Type ValueType { get; }

        object[] Attributes { get; }
    }
}