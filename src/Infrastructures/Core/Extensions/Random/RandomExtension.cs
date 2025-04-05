using System.Diagnostics;

namespace System;

public static class RandomExtension
{
    private static readonly char[] _constant =
[
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    'a',
    'b',
    'c',
    'd',
    'e',
    'f',
    'g',
    'h',
    'i',
    'j',
    'k',
    'l',
    'm',
    'n',
    'o',
    'p',
    'q',
    'r',
    's',
    't',
    'u',
    'v',
    'w',
    'x',
    'y',
    'z'
];

    private static readonly char[] _constantNumber =
    [
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9'
];

    /// <summary>
    /// Generates a truly random number.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="seed">The seed value</param>
    /// <returns>A random number</returns>
    public static int StrictNext(this Random _, int seed = int.MaxValue)
    {
        return new Random((int)Stopwatch.GetTimestamp()).Next(seed);
    }

    /// <summary>
    /// Generates a random number.
    /// </summary>
    ///  <param name="rand"></param>
    /// <param name="length">The length of the random number</param>
    /// <param name="isNumberOnly">Whether the random number is purely numeric</param>
    /// <returns>A random number</returns>
    public static string Next(this Random rand, int length, bool isNumberOnly)
    {
        char[] array;
        if (isNumberOnly)
        {
            array = _constantNumber;
        }
        else
        {
            array = _constant;
        }
        var stringBuilder = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            var index = rand.Next(array.Length);
            stringBuilder.Append(array[index]);
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Generates a random number following a normal distribution.
    /// </summary>
    /// <param name="rand"></param>
    /// <param name="mean">The mean (average) value</param>
    /// <param name="stdDev">The standard deviation</param>
    /// <returns>A random number following a normal distribution</returns>
    public static double NextGauss(this Random rand, double mean, double stdDev)
    {
        var u1 = 1.0 - rand.NextDouble();
        var u2 = 1.0 - rand.NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
