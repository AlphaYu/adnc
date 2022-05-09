namespace System;

public static class TimeSpanExtension
{
    /// <summary>
    ///     A TimeSpan extension method that substract the specified TimeSpan to the current DateTime.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The current DateTime with the specified TimeSpan substracted from it.</returns>
    public static DateTime Ago(this TimeSpan @this) => DateTime.Now.Subtract(@this);

    /// <summary>
    ///     A TimeSpan extension method that add the specified TimeSpan to the current DateTime.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The current DateTime with the specified TimeSpan added to it.</returns>
    public static DateTime FromNow(this TimeSpan @this) => DateTime.Now.Add(@this);

    /// <summary>
    ///     A TimeSpan extension method that substract the specified TimeSpan to the current UTC (Coordinated Universal
    ///     Time)
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The current UTC (Coordinated Universal Time) with the specified TimeSpan substracted from it.</returns>
    public static DateTime UtcAgo(this TimeSpan @this) => DateTime.UtcNow.Subtract(@this);

    /// <summary>
    ///     A TimeSpan extension method that add the specified TimeSpan to the current UTC (Coordinated Universal Time)
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The current UTC (Coordinated Universal Time) with the specified TimeSpan added to it.</returns>
    public static DateTime UtcFromNow(this TimeSpan @this) => DateTime.UtcNow.Add(@this);
}