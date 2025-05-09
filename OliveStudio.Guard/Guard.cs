using System.Linq.Expressions;

namespace OliveStudio;

/// <summary>
/// Provides guard clauses for validating method arguments.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the specified item is null.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="item">The item to check for null.</param>
    /// <returns>The original item if it is not null.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the item is null.</exception>
    public static T ThrowIfNull<T>(T item)
    {
        if (item is null)
        {
            throw new InvalidOperationException();
        }

        return item;
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the specified item is not null.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="item">The item to check for not null.</param>
    /// <returns>The original item if it is null.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the item is not null.</exception>
    public static T ThrowIfNotNull<T>(T item)
    {
        if (item is not null)
        {
            throw new InvalidOperationException();
        }

        return item;
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the specified integer is within the specified range.
    /// </summary>
    /// <param name="item">The integer to check.</param>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <returns>The original integer if it is not within the range.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the integer is within the range.</exception>
    public static int ThrowIfIntegerNotInRange(int item, int start, int end)
    {
        if (item >= start && item <= end)
        {
            throw new InvalidOperationException();
        }

        return item;
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the specified string is null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>The original string if it is not null or empty.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the string is null or empty.</exception>
    public static string ThrowIfStringNullOrEmpty(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException();
        }

        return value;
    }
}
