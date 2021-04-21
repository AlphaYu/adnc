using JetBrains.Annotations;
using System;
using System.Diagnostics;

namespace Adnc.Infra.Common.Extensions
{
    public static class RandomExtension
    {
        /// <summary>
        /// 生成真正的随机数
        /// </summary>
        /// <param name="r"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static int StrictNext(this Random r, int seed = int.MaxValue)
        {
            return new Random((int)Stopwatch.GetTimestamp()).Next(seed);
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

        /// <summary>
        ///     A Random extension method that return a random value from the specified values.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing arguments.</param>
        /// <returns>One of the specified value.</returns>
        public static T OneOf<T>([NotNull] this Random @this, params T[] values)
        {
            return values[@this.Next(values.Length)];
        }

        /// <summary>
        ///     A Random extension method that flip a coin toss.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true 50% of time, otherwise false.</returns>
        public static bool CoinToss([NotNull] this Random @this)
        {
            return @this.Next(2) == 0;
        }
    }
}
