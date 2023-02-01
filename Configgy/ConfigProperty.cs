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
        
        /// <inheritdoc />
        public NullabilityInfo NullabilityInfo { get; }
        
        /// <summary>
        /// Create a new <see cref="ConfigProperty"/>.
        /// </summary>
        /// <param name="valueName">The name of the config value.</param>
        /// <param name="property">The property on the config class that is being populated.</param>
        /// <param name="attributes">Any attributes associated with the config property being populated.</param>
        public ConfigProperty(string valueName, PropertyInfo property, IEnumerable<object>? attributes)
        {
            ValueName = valueName;
            ValueType = property.PropertyType;
            PropertyName = property.Name;
            NullabilityInfo = NullabilityInfoGetter.Get(property);
            Attributes = attributes == null
                ? Array.Empty<object>()
                : attributes as object[] ?? attributes.ToArray();

        }
    }
}