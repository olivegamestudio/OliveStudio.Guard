namespace OliveStudio;

public readonly struct GuardClause<T>(T value, string paramName)
{
    public void When(Func<T, bool> predicate, string message = "Invalid value.")
    {
        if (predicate(value))
        {
            throw new ArgumentException(message, paramName);
        }
    }

    public void WhenNull()
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
    
    public void WhenNotNull()
    {
        if (value is not null)
        {
            throw new ArgumentException(paramName);
        }
    }
}