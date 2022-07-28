using System.Diagnostics;

namespace System;

public static class RandomExtension
{
    private static readonly char[] Constant = new[]
{
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
};

    private static readonly char[] ConstantNumber = new[]
    {
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
};

    /// <summary>
    /// 生成真正的随机数
    /// </summary>
    /// <param name="_"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    public static int StrictNext(this Random _, int seed = int.MaxValue)
    {
        return new Random((int)Stopwatch.GetTimestamp()).Next(seed);
    }

    /// <summary>
    /// 生成随机数
    /// </summary>
    /// <param name="length">随机数长度</param>
    /// <param name="isNumberOnly">随机数是否是纯数字</param>
    /// <returns></returns>
    public static string Next(this Random rand, int length, bool isNumberOnly)
    {
        char[] array;
        if (isNumberOnly)
        {
            array = ConstantNumber;
        }
        else
        {
            array = Constant;
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
    /// 产生正态分布的随机数
    /// </summary>
    /// <param name="rand"></param>
    /// <param name="mean">均值</param>
    /// <param name="stdDev">方差</param>
    /// <returns></returns>
    public static double NextGauss(this Random rand, double mean, double stdDev)
    {
        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}