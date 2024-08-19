using System.Linq.Expressions;

namespace Utility;

public static class Guard
{
    public static T ThrowIfNull<T>(T item)
    {
        if (item is null)
        {
            throw new InvalidOperationException();
        }

        return item;
    }

    public static T ThrowIfNotNull<T>(T item)
    {
        if (item is not null)
        {
            throw new InvalidOperationException();
        }

        return item;
    }

    public static int ThrowIfIntegerNotInRange(int item, int start, int end)
    {
        if (item >= start && item <= end)
        {
            throw new InvalidOperationException();
        }

        return item;
    }

    public static string ThrowIfStringNullOrEmpty(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException();
        }

        return value;
    }
}
