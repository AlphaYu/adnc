namespace Adnc.Shared.Application.BloomFilter;

public interface IBloomFilter
{
    /// <summary>
    /// 过滤器名字
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 容错率
    /// </summary>
    double ErrorRate { get; }

    /// <summary>
    /// 容积
    /// </summary>
    int Capacity { get; }

    /// <summary>
    /// 初始化布隆过滤器
    /// </summary>
    /// <returns></returns>
    Task InitAsync();

    Task<bool> AddAsync(string value);

    Task<bool[]> AddAsync(IEnumerable<string> values);

    Task<bool> ExistsAsync(string value);

    Task<bool[]> ExistsAsync(IEnumerable<string> values);
}