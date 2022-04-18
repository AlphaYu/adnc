namespace Adnc.Shared.Application.BloomFilter;

public interface IBloomFilterFactory
{
    IBloomFilter GetBloomFilter(string name);
}