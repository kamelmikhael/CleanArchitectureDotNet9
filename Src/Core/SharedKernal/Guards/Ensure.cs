using System.Runtime.CompilerServices;

namespace SharedKernal.Guards;

public static class Ensure
{
    /// <summary>
    /// Ensure string not empty, null or white spaces
    /// </summary>
    /// <param name="value"></param>
    /// <param name="paramName"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void NotEmpty(
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value)) 
        { 
            throw new ArgumentException("The value is required", paramName); 
        }
    }

    /// <summary>
    /// Ensure value is not null
    /// </summary>
    /// <param name="value"></param>
    /// <param name="paramName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void NotNull(
        object? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}
