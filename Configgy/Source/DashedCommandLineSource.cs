using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from key-value pairs specified on the command line.
    /// </summary>
    public class DashedCommandLineSource : ValueSourceAttributeBase
    {
        private const string CommandLineParameterRegexValue = @"^\-\-(?<name>[a-z][a-z0-9]*)(?<equals>=(?<value>.*))?$";
        private static readonly Regex CommandLineParameterRegex = new Regex(CommandLineParameterRegexValue, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        private readonly IReadOnlyDictionary<string, string?> _values;

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

        /// <inheritdoc cref="IValueSource.Get"/>
        public override bool Get(IConfigProperty property, out string? value)
        {
            // See if the name exists in the dictionary
            if (!_values.ContainsKey(property.ValueName))
            {
                value = null;
                return false;
            }

            // Get the raw value from the dictionary
            value = _values[property.ValueName];

            // If the name is in the dictionary but contains no value and it is a boolean property then we assume it is true
            if (value == null && property.ValueType == typeof(bool)) value = "True";

            // Return the value
            return value != null;
        }
    }
}
