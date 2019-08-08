using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public ConfigProperty(string valueName, Type valueType, PropertyInfo property, IEnumerable<object> additionalAttributes)
        {
            ValueName = valueName;
            PropertyName = property?.Name;
            ValueType = valueType;

            Attributes = (additionalAttributes ?? Enumerable.Empty<object>())
                .Union(property?.GetCustomAttributes(true) ?? Enumerable.Empty<object>())
                .ToArray();
        }
    }
}