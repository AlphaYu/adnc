using Adnc.Infra.Core.Exceptions;

namespace Adnc.Infra.Core.Guard;

public static class GuardExtensions
{
    public static T GTZero<T>(this IGuard _, T value, string parameterName, string? message = null)
        where T : struct, IConvertible, IComparable<T>
    {
        var target = default(T);
        if (value.CompareTo(target) < 1)
            throw new BusinessException(message ?? $"{nameof(parameterName)} cannot be less than 0");
        return value;
    }

    public static string NotNullOrEmpty(this IGuard _, string value, string parameterName, string? message = null)
    {
        if (value.IsNullOrWhiteSpace())
            throw new BusinessException(message ?? $"{nameof(parameterName)} cannot be null or empty");
        return value;
    }

    public static T NotNullOrAny<T>(this IGuard _, T value, string parameterName, string? message = null)
        where T : ICollection
    {
        if (value is null || value.Count < 1)
            throw new BusinessException(message ?? $"{nameof(parameterName)} cannot be null or empty");
        return value;
    }

    public static T NotNull<T>(this IGuard _, T value, [NotNull] string parameterName, string? message = null)
        where T : class
    {
        if (value is null)
            throw new BusinessException(message ?? $"{nameof(parameterName)} cannot be null");

        return value;
    }

    public static void ThrowIf(this IGuard _, Func<bool> predicate, string message)
    {
        var result = predicate.Invoke();
        if (result)
            throw new BusinessException(message);
    }
}