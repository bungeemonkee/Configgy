using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Configgy.Source
{
    public class CommandLineSource : IValueSource
    {
        private const string CommandLineParameterRegexValue = @"^/c(onfig)?:(?<name>[a-z][a-z0-9]*)=(?<value>.+)$";
        private static readonly Regex CommandLineParameterRegex = new Regex(CommandLineParameterRegexValue, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        private readonly IReadOnlyDictionary<string, string> _values;

        public CommandLineSource(string[] commandLine)
        {
            // parse all the command line parameters and save any matching lines into the dictionary
            _values = commandLine.Select(c => CommandLineParameterRegex.Match(c))
                .Where(m => m.Success)
                .Select(m => m.Groups)
                .ToDictionary(g => g["name"].Value, g => g["value"].Value, StringComparer.InvariantCultureIgnoreCase);
        }

        public string GetRawValue(string valueName, PropertyInfo property)
        {
            // get the raw value from the dictionary
            string result;
            return _values.TryGetValue(valueName, out result)
                ? result
                : null;
        }
    }
}
