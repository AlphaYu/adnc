namespace Adnc.Demo.Admin.Application.Cache;

public class AccountBloomFilter(Lazy<IRedisProvider> redisProvider, Lazy<IDistributedLocker> distributedLocker, Lazy<IServiceProvider> serviceProvider)
    : AbstractBloomFilter(redisProvider, distributedLocker)
{
    public override string Name => CachingConsts.BloomfilterOfAccountsKey;

    public override double ErrorRate => 0.001;

    public override int Capacity => 10000000;

    public override async Task InitAsync()
    {
        var exists = await ExistsBloomFilterAsync();
        if (!exists)
        {
            using var scope = serviceProvider.Value.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<User>>();
            var values = await repository.GetAll()
                                         .Select(x => x.Account)
                                         .ToListAsync();
            await InitAsync(values);
        }
    }
}
