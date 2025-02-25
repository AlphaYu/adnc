namespace System;

public static class DoubleExtension
{
    public static long ToLong(this double @this) => Convert.ToInt64(@this);

    /// <summary>
    ///     Returns the smallest integral value that is greater than or equal to the specified double-precision floating-
    ///     point number.
    /// </summary>
    /// <param name="a">A double-precision floating-point number.</param>
    /// <returns>
    ///     The smallest integral value that is greater than or equal to . If  is equal to , , or , that value is
    ///     returned. Note that this method returns a  instead of an integral type.
    /// </returns>
    public static int Ceiling(this double @this) => Convert.ToInt32(Math.Ceiling(@this));

    /// <summary>
    ///     Returns the largest integer less than or equal to the specified double-precision floating-point number.
    /// </summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>The largest integer less than or equal to . If  is equal to , , or , that value is returned.</returns>
    public static int Floor(this double @this) => Convert.ToInt32(Math.Floor(@this));
}