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
        /// <param name="property">The <see cref="IConfigProperty"/> for this value.</param>
        /// <returns>The transformed configuration value.</returns>
        string Transform(IConfigProperty property, string value);
    }
}
