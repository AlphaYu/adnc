using Adnc.Infra.Core.Exceptions;

namespace Adnc.Infra.Core.Guard;

public partial class Checker
{
    internal Checker()
    {
    }

    public static T GTZero<T>(T value, string? parameterName = null, string? message = null)
    where T : struct, IConvertible, IComparable<T>
    {
        var target = default(T);
        if (value.CompareTo(target) < 1)
            throw new BusinessException(message ?? $"{parameterName ?? nameof(value)} cannot be less than 0");
        return value;
    }

    public static string NotNullOrEmpty(string value, string? parameterName = null, string? message = null)
    {
        if (value.IsNullOrWhiteSpace())
            throw new BusinessException(message ?? $"{parameterName ?? nameof(value)} cannot be null or empty");
        return value;
    }

    public static T NotNullOrAny<T>(T value, string? parameterName = null, string? message = null)
        where T : ICollection
    {
        if (value is null || value.Count < 1)
            throw new BusinessException(message ?? $"{parameterName ?? nameof(value)} cannot be null or empty");
        return value;
    }

    public static T NotNull<T>(T value, string? parameterName = null, string? message = null)
        where T : class
    {
        if (value is null)
            throw new BusinessException(message ?? $"{parameterName ?? nameof(value)} cannot be null");

        return value;
    }

    public static void ThrowIf(Func<bool> predicate, string message)
    {
        var result = predicate.Invoke();
        if (result)
            throw new BusinessException(message);
    }

    public static void ThrowIf<TExcetion>(Func<bool> predicate, TExcetion exception)
        where TExcetion : Exception, new()
    {
        var result = predicate.Invoke();
        if (result)
            throw exception;
    }

    public static void ThrowIfNull<T>(T value, string? variableName = null, string? message = null)
    {
        if (value is null)
            throw new NullReferenceException(message ?? $"{variableName ?? nameof(value)} cannot be null");
    }

    public static void ThrowIfNullOrEmpty<T>(T value, string? variableName = null, string? message = null)
        where T : ICollection
    {
        if (value is null || value.Count == 0)
            throw new NullReferenceException(message ?? $"{variableName ?? nameof(value)} cannot be null or empty");
    }

    public static void ThrowIfNullOrEmpty(string value, string? variableName = null, string? message = null)
    {
        if (value is null)
            throw new NullReferenceException(message ?? $"{variableName ?? nameof(value)} cannot be null");
    }

    public static void ThrowIfNullOrWhiteSpace(string? value, string? variableName = null, string? message = null)
    {
        if (value.IsNullOrWhiteSpace())
            throw new NullReferenceException(message ?? $"{variableName ?? nameof(value)} cannot be null or empty and ' '");
    }
}
