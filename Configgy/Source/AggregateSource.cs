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

        public AggregateSource()
            : this(new EnvironmentVariableSource(),
                  new ConectionStringsSource(),
                  new FileSource(),
                  new AppSettingSource(),
                  new DefaultValueAttributeSource())
        {
        }

        public AggregateSource(string[] commandLine)
            : this(new CommandLineSource(commandLine),
                  new EnvironmentVariableSource(),
                  new FileSource(),
                  new ConectionStringsSource(),
                  new AppSettingSource(),
                  new DefaultValueAttributeSource())
        {
        }

        public AggregateSource(params IValueSource[] sources)
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
