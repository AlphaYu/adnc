using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Infra.Caching
{
    public interface IBloomFilter
    {
        /// <summary>
        /// Creates an empty Bloom Filter with a single sub-filter for the initial capacity requested and with an upper bound error_rate . 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errorRate"></param>
        /// <param name="initialCapacity"></param>
        /// <returns></returns>
        Task BloomReserveAsync(string key, double errorRate, int initialCapacity);

        /// <summary>
        /// Adds an item to the Bloom Filter, creating the filter if it does not yet exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> BloomAddAsync(string key, string value);

        /// <summary>
        /// Adds one or more items to the Bloom Filter and creates the filter if it does not exist yet. 
        /// This command operates identically to BF.ADD except that it allows multiple inputs and returns multiple values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<bool[]> BloomAddAsync(string key, IEnumerable<string> values);

        /// <summary>
        /// Determines whether an item may exist in the Bloom Filter or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> BloomExistsAsync(string key, string value);

        /// <summary>
        /// Determines if one or more items may exist in the filter or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<bool[]> BloomExistsAsync(string key, IEnumerable<string> values);
    }
}
