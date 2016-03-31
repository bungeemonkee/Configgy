using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Configgy.Source
{
    /// <summary>
    /// A value source that uses embedded resources as configuration value sources.
    /// </summary>
    public class EmbeddedResourceSource : IValueSource
    {
        private const string DefaultExpression = @"(?:[a-zA-Z][a-zA-Z0-9]*\.)+Resources\.Settings\.(?<name>[a-zA-Z0-9][a-zA-Z0-9]*)(?:(?:\.conf)|(?:\.json)|(?:\.xml))";

        /// <summary>
        /// The expression used to determine if any embedded resource matches the 
        /// </summary>
        protected readonly Regex ResourceNameExpression;

        /// <summary>
        /// Creates a new EmbeddedResourceSource instance.
        /// The source will match the name of the value to any embedded resource that:
        /// <list type="ul">
        /// <item>Is in a namespace named 'Resources.Settings'.</item>
        /// <item>Has a file extension that is one of '.conf', '.json', '.xml'.</item>
        /// </list>
        /// </summary>
        public EmbeddedResourceSource()
            : this(DefaultExpression)
        { }

        /// <summary>
        /// Creates a new EmbeddedResourceSource instance with a specific expression to match resource names.
        /// </summary>
        /// <param name="resourceNameExpression">
        /// The expression used to match resource names from loaded assembly resources.
        /// In the case of a match the expression must produce a named group called 'name' that contains the name of the setting.
        /// </param>
        public EmbeddedResourceSource(string resourceNameExpression)
        {
            ResourceNameExpression = new Regex(resourceNameExpression);
        }

        /// <summary>
        /// Get the raw configuration value from an embedded resource.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <returns>The raw configuration value or null if there isn't an embedded resources that matches the value name.</returns>
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            // From all the loaded assemblies get any matching resources and return the first
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => !x.IsDynamic)
                .Select(x => new
                {
                    Assembly = x,
                    Resources = x.GetManifestResourceNames()
                })
                .SelectMany(x => x.Resources, (x, y) => new
                {
                    Assembly = x.Assembly,
                    Resource = y,
                    Match = ResourceNameExpression.Match(y)
                })
                .Where(x => x.Match.Success)
                .Where(x => x.Match.Groups["name"]?.Value == valueName)
                .Select(x => GetResourceText(x.Assembly, x.Resource))
                .Where(x => x != null)
                .FirstOrDefault();
        }

        /// <summary>
        /// Get an embedded resource as text.
        /// </summary>
        /// <param name="assembly">The assembly containing the resource.</param>
        /// <param name="name">The name of the resource.</param>
        /// <returns>The text of the resource or null if there was no such resource or it contained no text.</returns>
        protected string GetResourceText(Assembly assembly, string name)
        {
            try
            {
                using (var stream = assembly.GetManifestResourceStream(name))
                using (var textStream = new StreamReader(stream))
                {
                    return textStream.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
