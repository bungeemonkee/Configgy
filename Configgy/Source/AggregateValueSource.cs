using System.Linq;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// A value source that aggregates multiple other value sources.
    /// </summary>
    public class AggregateValueSource : IValueSource
    {
        private readonly IValueSource[] _sources;

        public AggregateValueSource()
            : this(new EnvironmentVariableValueSource(),
                  new ConectionStringsValueSource(),
                  new AppSettingValueSource(),
                  new DefaultValueAttributeValueSource())
        {
        }

        public AggregateValueSource(string[] commandLine)
            : this(new CommandLineValueSource(commandLine),
                  new EnvironmentVariableValueSource(),
                  new ConectionStringsValueSource(),
                  new AppSettingValueSource(),
                  new DefaultValueAttributeValueSource())
        {
        }

        public AggregateValueSource(params IValueSource[] sources)
        {
            _sources = sources;
        }

        public string GetRawValue(string valueName, PropertyInfo property)
        {
            return _sources
                .Select(s => s.GetRawValue(valueName, property))
                .Where(v => !string.IsNullOrEmpty(v))
                .FirstOrDefault();
        }
    }
}
