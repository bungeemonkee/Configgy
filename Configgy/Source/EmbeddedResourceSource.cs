﻿using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Configgy.Source
{
    /// <summary>
    /// A value source that uses embedded resources as configuration value sources.
    /// </summary>
    public class EmbeddedResourceSource : ValueSourceAttributeBase
    {
        private const string DefaultExpression = @"(?:[a-zA-Z_][a-zA-Z0-9_]*\.)+Resources\.Settings\.(?<name>[a-zA-Z0-9_][a-zA-Z0-9_]*)(?:(?:\.conf)|(?:\.config)|(?:\.txt)|(?:\.json)|(?:\.xml))";

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
        {
        }

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

        /// <inheritdoc cref="IValueSource.Get"/>
        public override bool Get(IConfigProperty property, out string value)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // From all the loaded assemblies get any matching resource
            var resource = assemblies
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
                .SingleOrDefault(x => x.Match.Groups["name"]?.Value == property.ValueName);

            // If there is no matching resource then return null
            if (resource == null)
            {
                value = null;
                return false;
            }

            // Get the value from the resource
            using (var stream = resource.Assembly.GetManifestResourceStream(resource.Match.Value))
            using (var textStream = new StreamReader(stream))
            {
                value = textStream.ReadToEnd();
                return true;
            }
        }
    }
}
