using System;

namespace Configgy.Source
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineNameAttribute : Attribute
    {
        public string CommandLineName { get; }

        public CommandLineNameAttribute(string commandLineName)
        {
            CommandLineName = commandLineName;
        }
    }
}
