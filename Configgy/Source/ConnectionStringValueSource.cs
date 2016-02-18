using System.Configuration;
using System.Reflection;

namespace Configgy.Source
{
    public class ConectionStringsValueSource : IValueSource
    {
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[valueName];
            if (connectionString == null) return null;
            return connectionString.ConnectionString;
        }
    }
}
