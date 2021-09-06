namespace Adnc.Application.Shared.BloomFilter
{
    public interface IBloomFilterFactory
    {
        IBloomFilter GetBloomFilter(string name);
    }
}
