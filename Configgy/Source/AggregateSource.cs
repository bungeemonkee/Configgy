﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// A value source that aggregates multiple other value sources.
    /// </summary>
    public class AggregateSource : ValueSourceAttributeBase
    {
        private readonly IValueSource[] _sources;

        /// <summary>
        /// The <see cref="IValueSource"/>s used.
        /// This does not include ones defined as property attributes.
        /// </summary>
        public IEnumerable<IValueSource> Sources => _sources;

        /// <summary>
        /// Creates an AggregateSource that delegates to the following sources in order:
        /// <list type="number">
        /// <item><see cref="EnvironmentVariableSource"/></item>
        /// <item><see cref="FileSource"/></item>
        /// <item><see cref="DefaultValueAttributeSource"/></item>
        /// </list>
        /// </summary>
        public AggregateSource()
            : this(new EnvironmentVariableSource(),
                  new FileSource(),
                  new DefaultValueAttributeSource())
        {
        }

        /// <summary>
        /// Creates an AggregateSource using the given command line that delegates to the following sources in order:
        /// <list type="number">
        /// <item><see cref="DashedCommandLineSource"/> using the given command line.</item>
        /// <item><see cref="EnvironmentVariableSource"/></item>
        /// <item><see cref="FileSource"/></item>
        /// <item><see cref="DefaultValueAttributeSource"/></item>
        /// </list>
        /// </summary>
        public AggregateSource(string[] commandLine)
            : this(new DashedCommandLineSource(commandLine),
                  new EnvironmentVariableSource(),
                  new FileSource(),
                  new DefaultValueAttributeSource())
        {
        }

        /// <summary>
        /// Creates an AggregateSource that delegates to the given sources in order.
        /// </summary>
        /// <param name="sources"></param>
        public AggregateSource(params IValueSource[] sources)
        {
            _sources = sources;
        }

        /// <summary>
        /// Gets a raw value from the sources used to create this aggregate source.
        /// See <see cref="IValueSource.Get"/>.
        /// </summary>
        public override bool Get(string valueName, PropertyInfo property, out string value)
        {
            ISet<Type> sourcesToIgnore = new HashSet<Type>();
            if (property != null)
            {
                sourcesToIgnore.UnionWith(((ICustomAttributeProvider)property)
                    .GetCustomAttributes(true)
                    .OfType<PreventSourceAttribute>()
                    .Select(x => x.SourceType)
                    .Where(x => x != null));
            }

            // Include sources specified as attributes if there are any
            var sources = (IEnumerable<IValueSource>)_sources;
            if (property != null)
            {
                sources = ((ICustomAttributeProvider)property)
                    .GetCustomAttributes(true)
                    .OfType<IValueSource>()
                    .Concat(_sources);
            }

            // Get each un-ignored source in turn
            foreach (var source in sources.Where(x => !sourcesToIgnore.Contains(x.GetType())))
            {
                // If a source has the value then return that value
                if (source.Get(valueName, property, out value)) return true;
            }

            // No source has the value
            value = null;
            return false;
        }
    }
}
