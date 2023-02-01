using System;

namespace Configgy.Exceptions;

/// <summary>
/// Exception thrown when a <see cref="Config"/> instance finds a value that fails nullable reference validation.
/// </summary>
public class NullableReferenceValidationException : Exception
{
    /// <summary>
    /// Creates a new NullableReferenceValidationException for the given value name.
    /// </summary>
    /// <param name="valueName">The name of the value that failed nullable reference exception.</param>
    public NullableReferenceValidationException(string valueName)
        : base(GetMessage(valueName))
    {
    }

    private static string GetMessage(string valueName)
    {
        const string format = "Property '{0}' failed nullable reference validaton.";

        return string.Format(format, valueName);
    }
}