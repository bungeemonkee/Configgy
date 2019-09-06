using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Configgy.Source;

namespace Configgy
{
    /// <inheritdoc />
    public class ConfigProperty : IConfigProperty
    {
        /// <inheritdoc />
        public string ValueName { get; }
        
        /// <inheritdoc />
        public string PropertyName { get; }
        
        /// <inheritdoc />
        public Type ValueType { get; }
        
        /// <inheritdoc />
        public object[] Attributes { get; }

        public ConfigProperty(string valueName, Type valueType, PropertyInfo property, IEnumerable<object> attributes)
        {
            ValueName = valueName;
            ValueType = valueType;
            PropertyName = property?.Name;
            Attributes = attributes == null
                ? new object[0]
                : attributes as object[] ?? attributes.ToArray();
        }
    }
}