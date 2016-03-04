using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from a parsed command line.
    /// </summary>
    public class CommandLineSource : IValueSource
    {
        private const string CommandLineParameterRegexValue = @"^/c(onfig)?:(?<name>[a-z][a-z0-9]*)=(?<value>.+)$";
        private static readonly Regex CommandLineParameterRegex = new Regex(CommandLineParameterRegexValue, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        private readonly IReadOnlyDictionary<string, string> _values;

        /// <summary>
        /// Creates a CommandLineSource with the given command line.
        /// </summary>
        /// <param name="commandLine">The raw command line to be parsed for configuration values.</param>
        public CommandLineSource(string[] commandLine)
        {
            // parse all the command line parameters and save any matching lines into the dictionary
            _values = commandLine.Select(c => CommandLineParameterRegex.Match(c))
                .Where(m => m.Success)
                .Select(m => m.Groups)
                .ToDictionary(g => g["name"].Value, g => g["value"].Value, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <returns>The raw configuration value or null if there isn't one in this source.</returns>
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
