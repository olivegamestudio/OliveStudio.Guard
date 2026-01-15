using System.Runtime.CompilerServices;

namespace OliveStudio;

/// <summary>
/// Provides guard clauses for validating method arguments.
/// </summary>
public static class Guard
{
    public static GuardClause<T> Against<T>(T value, [CallerArgumentExpression("value")] string paramName = "") 
        => new(value, paramName);
}
