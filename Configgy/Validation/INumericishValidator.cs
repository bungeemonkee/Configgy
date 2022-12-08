
namespace Configgy.Validation
{
    /// <summary>
    /// And extension of <see cref="IValueValidator"/> for number-like objects that includes validation for min and max values and/or a valid value range.
    /// </summary>
    /// <typeparam name="T">The numeric (ish) type.</typeparam>
    public interface INumericishValidator<out T> : IValueValidator
    {
        /// <summary>
        /// The minimum allowed value.
        /// </summary>
        T Min { get; }

        /// <summary>
        /// The maximum allowed value.
        /// </summary>
        T Max { get; }

        /// <summary>
        /// The list of allowed valid values.
        /// </summary>
        T[]? ValidValues { get; }
    }
}
