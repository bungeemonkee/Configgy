using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Configgy.Exceptions;
using Configgy.Source;

namespace Configgy
{
    /// <summary>
    /// Extensions for <see cref="Config"/> instances.
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Validates every configuration property. 
        /// </summary>
        /// <remarks>
        /// Works by dynamically invoking every public readable property on this instance.
        /// This will aggregate and re-throw any exceptions encountered within a single exception derived from AggregateException.
        /// If this completed successfully all configuration values will also be cached as a result.
        /// </remarks>
        /// <exception cref="ConfigValidationException">Thrown when there are one or more validation errors with properties.</exception>
        public static void Validate(this Config config)
        {
            // get all the public readable properties on this configuration type
            var properties = config
                .GetType()
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .OfType<PropertyInfo>()
                .Where(p => p.CanRead)
                .ToList();

            // if there are no properties to verify then return
            if (properties.Count == 0) return;

            var exceptions = new Dictionary<string, Exception>();

            // go through each property
            foreach (var prop in properties)
            {
                try
                {
                    // invoke the property
                    prop.GetValue(config);
                }
                catch (Exception e)
                {
                    // catch any exceptions
                    exceptions.Add(prop.Name, e);
                }
            }

            // if there were exceptions...
            if (exceptions.Count > 0)
            {
                // re-throw the exceptions
                throw new ConfigValidationException(exceptions);
            }
        }

        /// <summary>
        /// Gets a command line help message based on <see cref="HelpAttribute"/>s.
        /// </summary>
        /// <param name="config">The <see cref="Config"/> instance to get help for.</param>
        /// <returns>A string of help text formatted for the current console window.</returns>
        public static string GetCommandLineHelp(this Config config)
        {
            return GetCommandLineHelp(config, false);
        }

        /// <summary>
        /// Gets a command line help message based on <see cref="HelpAttribute"/>s.
        /// </summary>
        /// <param name="config">The <see cref="Config"/> instance to get help for.</param>
        /// <param name="includeUndocumented">True to include properties that have no <see cref="HelpAttribute"/>.</param>
        /// <returns>A string of help text formatted for the current console window.</returns>
        public static string GetCommandLineHelp(this Config config, bool includeUndocumented)
        {
            const int defaultWidth = 80;

            int width;
            if (Environment.UserInteractive)
            {
                try
                {
                    width = Console.WindowWidth;
                }
                catch
                {
                    try
                    {
                        width = Console.BufferWidth;
                    }
                    catch
                    {
                        width = defaultWidth;
                    }
                }
            }
            else
            {
                try
                {
                    width = Console.BufferWidth;
                }
                catch
                {
                    width = defaultWidth;
                }
            }

            return GetCommandLineHelp(config, includeUndocumented, width);
        }

        /// <summary>
        /// Gets a command line help message based on <see cref="HelpAttribute"/>s.
        /// </summary>
        /// <param name="config">The <see cref="Config"/> instance to get help for.</param>
        /// <param name="includeUndocumented">True to include properties that have no <see cref="HelpAttribute"/>.</param>
        /// <param name="width">The width (in characters) at which to wrap the help text.</param>
        /// <returns>A string of help text formatted for the current console window.</returns>
        public static string GetCommandLineHelp(this Config config, bool includeUndocumented, int width)
        {
            const int pad = 8;

            var boolType = typeof(bool);
            var commandLineSourceType = typeof(DashedCommandLineSource);
            var configType = config.GetType();

            var sb = new StringBuilder();

            // Get the properties that are going to be documented
            var properties = configType
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .OfType<PropertyInfo>()
                .Where(x => x.CanRead)
                .Select(x => new
                {
                    Property = x,
                    Attributes = x.GetCustomAttributes(true)
                })
                // Ignore properties that cannot come from the command line
                .Where(x => x.Attributes.OfType<PreventSourceAttribute>().All(y => y.SourceType != commandLineSourceType))
                .Select(x => new
                {
                    Property = x.Property,
                    HelpAttribute = x.Attributes.OfType<HelpAttribute>().FirstOrDefault()
                })
                // Get properties that have a help attribute (or all of them if we're including undocumented properties)
                .Where(x => includeUndocumented || x.HelpAttribute != null)
                .OrderBy(x => x.Property.Name)
                .ToList();

            // Get the executable and add it to the output
            var executable = Assembly.GetEntryAssembly()?.Location;
            if (!string.IsNullOrWhiteSpace(executable))
            {
                executable = Path.GetFileName(executable);
                sb.Append(executable);
                if (properties.Count > 0)
                {
                    sb.Append(" [OPTIONS]");
                }
                sb.AppendLine();
            }

            // Get any help text from the config class itself
            var description = configType
                .GetCustomAttributes(true)
                .OfType<HelpAttribute>()
                .FirstOrDefault()
                ?.HelpText;
            if (!string.IsNullOrWhiteSpace(description))
            {
                if (!string.IsNullOrWhiteSpace(executable))
                {
                    sb.AppendLine();
                }
                PadAndWrap(description, 0, width, sb);
                sb.AppendLine();
            }

            // Print every property
            foreach (var property in properties)
            {
                // Print the property name and value type
                sb.AppendLine();
                sb.Append("--");
                sb.Append(property.Property.Name);
                if (property.Property.PropertyType == boolType)
                {
                    // Bool properties can have a second syntax that omits the value
                    sb.Append(" OR --");
                    sb.Append(property.Property.Name);
                }
                sb.Append("=<");
                sb.Append(property.Property.PropertyType.Name);
                if (property.Property.PropertyType.IsEnum)
                {
                    // For enums also print the enum values
                    sb.Append(':');
                    sb.Append(string.Join(",", Enum.GetNames(property.Property.PropertyType)));
                }
                sb.AppendLine(">");

                // Print the property description
                var propertyDescription = property?.HelpAttribute.HelpText;
                if (string.IsNullOrWhiteSpace(propertyDescription)) continue;
                PadAndWrap(propertyDescription, pad, width, sb);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static void PadAndWrap(string text, int leftPad, int width, StringBuilder sb)
        {
            var currentWidth = 0;
            var words = Regex.Split(text, @"\s+");

            foreach (var word in words)
            {
                // If this word will not fit on this line...
                if (currentWidth != 0 && currentWidth + 1 + word.Length > width)
                {
                    sb.AppendLine();
                    currentWidth = 0;
                }

                // If this is a new line...
                if (currentWidth == 0)
                {
                    // Add the padding
                    sb.Append(' ', leftPad);

                    // Set the current width to the pad length
                    currentWidth = leftPad;
                }
                else
                {
                    sb.Append(' ');
                }

                // Add the word
                sb.Append(word);

                // Record the new length
                currentWidth += 1 + word.Length;
            }
        }
    }
}
