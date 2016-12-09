using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Configgy.Source
{
    public class DashedCommandLineSource : IValueSource
    {
        private const string CommandLineParameterRegexValue = @"^\-\-(?<name>[a-z][a-z0-9]*)(?<equals>=(?<value>.*))?$";
        private static readonly Regex CommandLineParameterRegex = new Regex(CommandLineParameterRegexValue, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        private readonly IReadOnlyDictionary<string, string> _values;

        /// <summary>
        /// Creates a DashedCommandLineSource with the given command line.
        /// </summary>
        /// <param name="commandLine">The raw command line to be parsed for configuration values.</param>
        public DashedCommandLineSource(string[] commandLine)
        {
            // Parse all the command line parameters and save any matching lines into the dictionary
            _values = commandLine.Select(c => CommandLineParameterRegex.Match(c))
                .Where(m => m.Success)
                .Select(m => m.Groups)
                .ToDictionary(g => g["name"].Value, g => g["equals"].Value == string.Empty ? null : g["value"].Value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <param name="value">The value found in the source.</param>
        /// <returns>True if the config value was found in the source, false otherwise.</returns>
        public bool Get(string valueName, PropertyInfo property, out string value)
        {
            // Check for command line name overrides
            valueName = property
                .GetCustomAttributes(true)
                .OfType<CommandLineNameAttribute>()
                .Select(x => x.CommandLineName)
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?? valueName;

            // See if the name exists in the dictionary
            if (!_values.ContainsKey(valueName))
            {
                value = null;
                return false;
            }

            // Get the raw value from the dictionary
            value = _values[valueName];

            // If the name is in the dictionary but contains no value and it is a boolean property then we assume it is true
            if (value == null && property.PropertyType == typeof(bool)) value = "True";

            // Return the value
            return value != null;
        }
    }
}
