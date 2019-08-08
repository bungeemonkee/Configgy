using System;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from the system's environment variables.
    /// </summary>
    public class EnvironmentVariableSource : ValueSourceAttributeBase
    {
        /// <inheritdoc cref="IValueSource.Get"/>
        public override bool Get(IConfigProperty property, out string value)
        {
            value = Environment.GetEnvironmentVariable(property.ValueName);
            return value != null;
        }
    }
}
