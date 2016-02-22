using System;
using System.Reflection;

namespace Configgy.Source
{
    public class EnvironmentVariableSource : IValueSource
    {
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            return Environment.GetEnvironmentVariable(valueName);
        }
    }
}
