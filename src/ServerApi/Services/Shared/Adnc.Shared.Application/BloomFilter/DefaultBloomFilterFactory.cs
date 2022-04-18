namespace Adnc.Shared.Application.BloomFilter;

public class DefaultBloomFilterFactory : IBloomFilterFactory
{
    private readonly IEnumerable<IBloomFilter> _filters;

    public DefaultBloomFilterFactory(IEnumerable<IBloomFilter> filters)
        => _filters = filters;

    public IBloomFilter GetBloomFilter(string name)
    {
        ArgumentCheck.NotNullOrWhiteSpace(name, nameof(name));

        var provider = _filters.FirstOrDefault(x => x.GetType().Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
            throw new ArgumentException("can not find a match bloom filters!");

        return provider;
    }
}