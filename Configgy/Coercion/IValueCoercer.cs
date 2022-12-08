namespace Configgy.Coercion
{
    /// <summary>
    /// Defines a value coercer - an object that can take raw string values and convert them into a specific type of object.
    /// </summary>
    public interface IValueCoercer
    {
        /// <summary>
        /// Coerce the raw string value into the expected result type.
        /// </summary>
        /// <typeparam name="T">The expected result type after coercion.</typeparam>
        /// <param name="value">The raw string value to be coerced.</param>
        /// <param name="property">The <see cref="IConfigProperty"/> for this value.</param>
        /// <param name="result">The coerced value.</param>
        /// <returns>True if the value could be coerced, false otherwise.</returns>
        bool Coerce<T>(IConfigProperty property, string? value, out T result);
    }
}
