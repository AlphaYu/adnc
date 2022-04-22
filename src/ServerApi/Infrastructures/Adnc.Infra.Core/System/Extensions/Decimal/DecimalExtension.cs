namespace System;

public static class DecimalExtension
{
    /// <summary>
    ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
    public static bool InRange(this decimal @this, decimal minValue, decimal maxValue) => @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
}
