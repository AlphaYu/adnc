namespace Adnc.Infra.Core.Guard;

public class ArgumentChecker
{
    private static readonly ArgumentChecker _instance = new();

    static ArgumentChecker()
    {
    }

    private ArgumentChecker()
    {
    }

    internal static ArgumentChecker Instance => _instance;

    /// <summary>
    /// Throw if the argument LE Zero.
    /// </summary>
    /// <param name="argument">Argument.</param>
    /// <param name="argumentName">Argument name.</param>
    /// <param name="message">message.</param>
    public void ThrowIfLEZero(long argument, string argumentName, string? message = null)
    {
        if (argument <= 0)
        {
            throw new ArgumentOutOfRangeException(argumentName, message: message);
        }
    }

    /// <summary>
    ///  Throw if the argument count LE Zero.
    /// </summary>
    /// <param name="argument">Argument.</param>
    /// <param name="argumentName">Argument name.</param>
    /// <param name="message">message.</param>
    public void ThrowIfLEZero(TimeSpan argument, string argumentName, string? message = null)
    {
        if (argument <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(argumentName, message: message);
        }
    }

    /// <summary>
    ///  Throw if the argument is  null or count LE Zero.
    /// </summary>
    /// <param name="argument">Argument.</param>
    /// <param name="argumentName">Argument name.</param>
    /// <param name="message">message.</param>
    public void ThrowIfNullOrCountLEZero<T>(IEnumerable<T>? argument, string argumentName, string? message = null)
    {
        if (argument is null || !argument.Any())
        {
            throw new ArgumentNullException(argumentName, message: message);
        }
    }

    /// <summary>
    ///  Throw if the argument is  null or count LE Zero.
    /// </summary>
    /// <param name="argument">Argument.</param>
    /// <param name="argumentName">Argument name.</param>
    /// <param name="message">message.</param>
    public void ThrowIfNullOrCountLEZero<T>(IDictionary<string, T>? argument, string argumentName, string? message = null)
    {
        if (argument is null || argument.Count <= 0)
        {
            throw new ArgumentNullException(argumentName, message: message);
        }
    }

    public void ThrowIfInvalidDate(DateTime argument, string argumentName)
    {
        var MinDate = new DateTime(1900, 1, 1);
        var MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        if (argument > MaxDate && argument < MinDate)
        {
            throw new ArgumentOutOfRangeException(argumentName);
        }
    }

    /// <summary>
    /// Not equal the length
    /// </summary>
    /// <param name="sourceLength"></param>
    /// <param name="limitLength"></param>
    /// <param name="argumentName"></param>
    public void ThrowIfNotEqualLength(int sourceLength, int limitLength, string argumentName)
    {
        if (limitLength != sourceLength)
        {
            throw new ArgumentException(argumentName, string.Format("The length of arugment {0} must be equal to {1}.", argumentName, limitLength));
        }
    }

    /// <summary>
    ///  argument length must be in the range
    /// </summary>
    /// <param name="argument"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="argumentName"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ThrowIfOutOfRange(int argument, int min, int max, string argumentName)
    {
        if ((argument < min) || (argument > max))
        {
            throw new ArgumentOutOfRangeException(argumentName, string.Format("{0} must be in the range \"{1}\"-\"{2}\".", argumentName, min, max));
        }
    }
}
