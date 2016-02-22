using System.Configuration;
using System.Reflection;

namespace Configgy.Source
{
    public class AppSettingSource : IValueSource
    {
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            return ConfigurationManager.AppSettings[valueName] as string;
        }
    }
}
