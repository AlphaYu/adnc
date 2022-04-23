namespace Adnc.Infra.Core.Guard;

public static class GuardExtensions
{
    public static T GTZero<T>(this IGuard _, T value, string parameterName)
        where T : struct, IConvertible, IComparable<T>
    {
        var target = default(T);
        if (value.CompareTo(target) < 1)
            throw new ArgumentException("不能小于0", parameterName);
        return value;
    }

    public static string NotNullOrEmpty(this IGuard _, string value, string parameterName)
    {
        if (value.IsNullOrWhiteSpace())
            throw new ArgumentNullException(parameterName);
        return value;
    }

    public static T NotNullOrAny<T>(this IGuard _, T value, string parameterName)
        where T : ICollection
    {
        if (value is null || value.Count < 1)
            throw new ArgumentNullException(parameterName);
        return value;
    }

    public static T NotNull<T>(this IGuard _, T value, string parameterName)
        where T : class
    {
        return NotNull(_, value, parameterName, string.Empty);
    }

    public static T NotNull<T>(this IGuard _, T value, [NotNull] string parameterName, string message)
        where T : class
    {
        if (value is null)
            throw new ArgumentNullException(parameterName, message);

        return value;
    }

}