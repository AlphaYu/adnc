using Adnc.Infra.Core.Exceptions;

namespace Adnc.Infra.Core.Guard;

public class VariableChecker
{
    private static readonly VariableChecker _instance = new();

    static VariableChecker()
    {
    }

    private VariableChecker()
    {
    }

    internal static VariableChecker Instance => _instance;

    public T GTZero<T>(T value, string? variablerName = null, string? message = null) where T : struct, IConvertible, IComparable<T>
    {
        var target = default(T);
        if (value.CompareTo(target) < 1)
        {
            throw new InvalidVariableException(message ?? $"{variablerName ?? nameof(value)} cannot be less than 0");
        }

        return value;
    }

    public string NotNullOrWhiteSpace(string? value, string? variablerName = null, string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidVariableException(message ?? $"{variablerName ?? nameof(value)} cannot be null or empty");
        }

        return value;
    }

    public T NotNullOrAny<T>(T? value, string? variablerName = null, string? message = null)
        where T : ICollection
    {
        if (value is null || value.Count < 1)
        {
            throw new InvalidVariableException(message ?? $"{variablerName ?? nameof(value)} cannot be null or empty");
        }

        return value;
    }

    public T NotNull<T>(T? value, string? variablerName = null, string? message = null)
        where T : class
    {
        if (value is null)
        {
            throw new InvalidVariableException(message ?? $"{variablerName ?? nameof(value)} cannot be null");
        }

        return value;
    }
}
