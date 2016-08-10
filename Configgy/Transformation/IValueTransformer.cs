using System.Reflection;

namespace Configgy.Transformation
{
    /// <summary>
    /// Defines a value transformer - an object that can take raw string values an perform some trasnformation on them.
    /// </summary>
    public interface IValueTransformer
    {
        /// <summary>
        /// A simple ordering mechanism used to ensure trasnformers are chained in the correct sequence.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Transform the configuration value.
        /// </summary>
        /// <param name="value">The raw string value to be transformed.</param>
        /// <param name="valueName">The name of the value to be transformed.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <returns>The transformed configuration value.</returns>
        string Transform(string value, string valueName, PropertyInfo property);
    }
}
