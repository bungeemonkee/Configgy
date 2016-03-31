using System.Linq;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// A value source that aggregates multiple other value sources.
    /// </summary>
    public class AggregateSource : IValueSource
    {
        private readonly IValueSource[] _sources;

        /// <summary>
        /// Creates an AggregateSource that delegates to the following sources in order:
        /// <list type="number">
        /// <item><see cref="EnvironmentVariableSource"/></item>
        /// <item><see cref="FileSource"/></item>
        /// <item><see cref="ConectionStringsSource"/></item>
        /// <item><see cref="AppSettingSource"/></item>
        /// <item><see cref="EmbeddedResourceSource"/></item>
        /// <item><see cref="DefaultValueAttributeSource"/></item>
        /// </list>
        /// </summary>
        public AggregateSource()
            : this(new EnvironmentVariableSource(),
                  new FileSource(),
                  new ConectionStringsSource(),
                  new AppSettingSource(),
                  new EmbeddedResourceSource(),
                  new DefaultValueAttributeSource())
        {
        }

        /// <summary>
        /// Creates an AggregateSource using the given command line that delegates to the following sources in order:
        /// <list type="number">
        /// <item><see cref="CommandLineSource"/> using the given command line.</item>
        /// <item><see cref="EnvironmentVariableSource"/></item>
        /// <item><see cref="FileSource"/></item>
        /// <item><see cref="ConectionStringsSource"/></item>
        /// <item><see cref="AppSettingSource"/></item>
        /// <item><see cref="EmbeddedResourceSource"/></item>
        /// <item><see cref="DefaultValueAttributeSource"/></item>
        /// </list>
        /// </summary>
        public AggregateSource(string[] commandLine)
            : this(new CommandLineSource(commandLine),
                  new EnvironmentVariableSource(),
                  new FileSource(),
                  new ConectionStringsSource(),
                  new AppSettingSource(),
                  new EmbeddedResourceSource(),
                  new DefaultValueAttributeSource())
        {
        }

        /// <summary>
        /// Creates an AggregateSource that delegates to the given sources in order.
        /// </summary>
        /// <param name="sources"></param>
        public AggregateSource(params IValueSource[] sources)
        {
            _sources = sources;
        }

        /// <summary>
        /// Gets a raw value from the sources used to create this aggregate source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">The property reference associated with the property, or null if none exists.</param>
        /// <returns></returns>
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            return _sources
                .Select(s => s.GetRawValue(valueName, property))
                .Where(v => !string.IsNullOrEmpty(v))
                .FirstOrDefault();
        }
    }
}
