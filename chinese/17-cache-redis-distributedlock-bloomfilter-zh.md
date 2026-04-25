## ADNC 如何使用Cache/Redis/分布式锁/布隆过滤器

[GitHub 仓库地址](https://github.com/alphayu/adnc)
.NET 官方提供了 `Microsoft.Extensions.Caching.Distributed.IDistributedCache` 接口，并且提供了 `StackExchange.Redis` 等实现。那么，为什么还需要再封装一层？
本文的出发点主要有两点：

1) `IDistributedCache` 的方法签名偏通用，在实际生产环境中往往无法直接覆盖项目需求，即使直接使用通常也需要二次封装。
2) 在项目中经常需要使用 Redis 的其他数据结构与能力。无论使用 StackExchange.Redis 还是 CSRedis，通常也需要统一封装以降低耦合。因此，`Adnc.Infra.Caching` 基于 StackExchange.Redis 进行了封装，用于管理和使用 cache、Redis 以及分布式锁。

> `Adnc.Infra.Caching` 参考拷贝了`EasyCaching`很多代码，并删减了大量`EasyCaching`代码同时也在`EasyCaching`的基础上完善了很多核心功能。

```csharp
//Microsoft.Extensions.Caching.Distributed.IDistributedCache
byte[] Get(string key);
Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken));
void Set(string key, byte[] value, DistributedCacheEntryOptions options);
Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token);
void Refresh(string key);
Task RefreshAsync(string key, CancellationToken token = default(CancellationToken));
void Remove(string key);
Task RemoveAsync(string key, CancellationToken token = default(CancellationToken));
```
## 总体设计
`Adnc.Infra.Redis/Adnc.Infra.Redis.Caching` 提供了三个接口以及两个Cache拦截器。

#### 接口
- IDistributedLocker 分布式锁
- IRedisProvider Redis操作接口
- ICacheProvider Cache操作接口

#### cache拦截器
- CachingEvictAttribute 删除拦截器
- CachingAbleAttribute 读取拦截器

## appsettings.json 配置示例
```json
"Redis": {
    //防止 cache 雪崩配置。保存 cache 时，过期时间会加上一个不超过 MaxRdSecond 的随机秒数。
    "MaxRdSecond": 30,
    //防止 cache 击穿配置。LockMs 为获取到分布式锁后的锁定时长；SleepMs 为未获取锁时的休眠时长，具体实现请参考源码。
    "LockMs": 6000,
    "SleepMs": 300,
    //cache 序列化配置。目前仅实现了 binary 一种方式，可自行扩展。
    "SerializerName": "binary",
    //cache 是否允许记录日志
    "EnableLogging": true,
    //Polly 超时时间。cache 与数据库同步补偿机制会用到该参数，具体实现请参考源码。
    "PollyTimeoutSeconds": 11,
    //防止 cache 穿透配置
    "PenetrationSetting": {
        //Disable=true 允许穿透
        //Disable=false不允许穿透
        "Disable": false,
        //布隆过滤器配置。cache 防穿透通过布隆过滤器实现。
        "BloomFilterSetting": {
            //过滤器名字
            "Name": "adnc:usr:bloomfilter:cachekeys",
            //大小
            "Capacity": 10000000,
            //容错率
            "ErrorRate": 0.001
            }
    },
    //Redis 连接串
    "Dbconfig": {
        "ConnectionString": "127.0.0.1:13379,password=football,defaultDatabase=11,ssl=false,sslHost=null,connectTimeout=4000,allowAdmin=true"
    }
}
```
## IDistributedLocker 
该接口提供一个安全的分布式锁，释放锁通过lua脚本+版本号(lockvalue)的方式释放，并且实现了自动续期的功能。
``` csharp
public interface IDistributedLocker
{
    /// <summary>
    /// 获取分布式锁
    /// </summary>
    /// <param name="cacheKey">cacheKey.</param>
    /// <param name="timeoutSeconds">锁定时间</param>
    /// <param name="autoDelay">是否自动续期</param>
    /// <returns>Success 获取锁的状态，LockValue锁的版本号</returns>
    Task<(bool Success, string LockValue)> LockAsync(string cacheKey, int timeoutSeconds = 5, bool autoDelay = true);

    /// <summary>
    /// 安全解锁
    /// </summary>
    /// <param name="cacheKey">cacheKey.</param>
    /// <param name="cacheValue">版本号</param>
    /// <returns></returns>
    Task<bool> SafedUnLockAsync(string cacheKey, string cacheValue);

    /// <summary>
    /// 获取分布式锁
    /// </summary>
    /// <param name="cacheKey">cacheKey.</param>
    /// <param name="timeoutSeconds">锁定时间</param>
    /// <param name="autoDelay">是否自动续期</param>
    /// <returns>Success 获取锁的状态，LockValue锁的版本号</returns>
    (bool Success, string LockValue) Lock(string cacheKey, int timeoutSeconds = 5, bool autoDelay = true);

    /// <summary>
    /// 安全解锁
    /// </summary>
    /// <param name="cacheKey">cacheKey.</param>
    /// <param name="cacheValue">版本号</param>
    /// <returns></returns>
    bool SafedUnLock(string cacheKey, string cacheValue);
}
```
### IDistributedLocker使用
在需要使用分布式锁的地方通过构造函数注入。

```csharp
public class xxxAppService
{
    private readonly IDistributedLocker _locker;
    public xxxAppService(IDistributedLocker locker)
    {
        _locker = locker;
    }
    
    public void Test()
    {
        var cacheKey = "adnc:menus";
        var flag = _locker.Lock(cacheKey);
        if(!flag.Success)
        {
            //获取锁失败
            //你的逻辑代码
            return;
        }

        //获取锁成功
        try
        {
            //你的逻辑代码
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message,ex);
        }
        finally
        {
            //一定要释放锁
            _locker.SafedUnLock(cacheKey,flag.LockValue);
        }
    }
}
```
## IRedisProvider 
默认基于StackExchange.Redis实现了该接口，该接口提供Redis所有数据类型的操作方法。IRedisProvidert提供的方法太多了，这里就不放全部代码了。贴下布隆拦截器的几个方法。

```csharp
public interface IRedisProvide
{
    //序列化方式，默认实现了binary，可以自己扩展。
    ICachingSerializer Serializer { get; }
   
    //其他方法

    #region Bloom Filter
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
    #endregions

}     
```
### IRedisProvider使用
在需要使用redis的地方通过构造函数注入。基本常用的方法大家应该都会，下面示例代码中演示了一个复杂点的，如何执行lua脚本。

```csharp
public class xxxAppService
{
    private readonly IRedisProvider _redis;
    public xxxAppService(IRedisProvider redis)
    {
        _redis = redis;
    }
    
    public void Test()
    {
        var cacheKey = "adnc:workerid"

        var scirpt = @"local workerids = redis.call('ZRANGE', @key, @start,@stop)
        redis.call('ZADD',@key,@score,workerids[1])
        return workerids[1]";

        var parameters = new { key = cacheKey, start = 0, stop = 0, score = DateTime.Now.GetTotalMilliseconds() };
        //执行Lua脚本
        var luaResult = (byte[]) await _redis.ScriptEvaluateAsync(scirpt, parameters);
        var workerId = _redis.Serializer.Deserialize<long>(luaResult);        
    }
}
```
## ICacheProvider
默认基于StackExchange.Redis实现了该接口,并解决了缓存雪崩/击穿/穿透/同步问题。该接口提供丰富cache相关的操作方法

```csharp
public interface ICacheProvider
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    CacheOptions CacheOptions { get; }

    /// <summary>
    /// The serializer.
    /// </summary>
    ICachingSerializer Serializer { get; }

    /// <summary>
    /// Set the specified cacheKey, cacheValue and expiration.
    /// </summary>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Sets the specified cacheKey, cacheValue and expiration async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Get the specified cacheKey.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    CacheValue<T> Get<T>(string cacheKey);

    /// <summary>
    /// Get the specified cacheKey async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task<CacheValue<T>> GetAsync<T>(string cacheKey);

    /// <summary>
    /// Remove the specified cacheKey.
    /// </summary>
    /// <param name="cacheKey">Cache key.</param>
    void Remove(string cacheKey);

    /// <summary>
    /// Remove the specified cacheKey async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    Task RemoveAsync(string cacheKey);

    /// <summary>
    /// Exists the specified cacheKey async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    Task<bool> ExistsAsync(string cacheKey);

    /// <summary>
    /// Exists the specified cacheKey.
    /// </summary>
    /// <returns>The exists.</returns>
    /// <param name="cacheKey">Cache key.</param>
    bool Exists(string cacheKey);

    /// <summary>
    /// Tries the set.
    /// </summary>
    /// <returns><c>true</c>, if set was tryed, <c>false</c> otherwise.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    bool TrySet<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Tries the set async.
    /// </summary>
    /// <returns>The set async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task<bool> TrySetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Sets all.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    void SetAll<T>(IDictionary<string, T> value, TimeSpan expiration);

    /// <summary>
    /// Sets all async.
    /// </summary>
    /// <returns>The all async.</returns>
    /// <param name="value">Value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task SetAllAsync<T>(IDictionary<string, T> value, TimeSpan expiration);

    /// <summary>
    /// Removes all.
    /// </summary>
    /// <param name="cacheKeys">Cache keys.</param>
    void RemoveAll(IEnumerable<string> cacheKeys);

    /// <summary>
    /// Removes all async.
    /// </summary>
    /// <returns>The all async.</returns>
    /// <param name="cacheKeys">Cache keys.</param>
    Task RemoveAllAsync(IEnumerable<string> cacheKeys);

    /// <summary>
    /// Get the specified cacheKey, dataRetriever and expiration.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="dataRetriever">Data retriever.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    CacheValue<T> Get<T>(string cacheKey, Func<T> dataRetriever, TimeSpan expiration);

    /// <summary>
    /// Gets the specified cacheKey, dataRetriever and expiration async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="dataRetriever">Data retriever.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task<CacheValue<T>> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan expiration);

    /// <summary>
    /// Removes cached item by cachekey's prefix.
    /// </summary>
    /// <param name="prefix">Prefix of CacheKey.</param>
    void RemoveByPrefix(string prefix);

    /// <summary>
    /// Removes cached item by cachekey's prefix async.
    /// </summary>
    /// <param name="prefix">Prefix of CacheKey.</param>
    Task RemoveByPrefixAsync(string prefix);

    /// <summary>
    /// Gets the specified cacheKey async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="type">Object Type.</param>
    Task<object> GetAsync(string cacheKey, Type type);

    /// <summary>
    /// Set the keys  TTL
    /// </summary>
    /// <param name="cacheKeys">Cache keys.</param>
    /// <param name="seconds">Expiration .</param>
    /// <returns></returns>
    Task KeyExpireAsync(IEnumerable<string> cacheKeys, int seconds);
}
```
### ICacheProvider 使用
在需要使用缓存的地方，可通过构造函数注入 `ICacheProvider`。在 ADNC 中，通常通过 `CacheService` 对缓存访问进行统一封装；你也可以在 `XXXAppService` 中直接注入并调用。

建议优先通过 `CacheService` 方式操作缓存，以便集中管理缓存键规范、过期策略、布隆过滤器与分布式锁等横切能力。

- 直接使用
```csharp
public class xxxAppService
{
    private readonly ICacheProvider _cache;
    public xxxAppService(ICacheProvider cache)
    {
        _cache = cache;
    }

    public void Test()
    {
        var cacheKey ="adnc:userinfo:account";
        var value = "alpha2008";
        await _cache.Value.Set(cacheKey, value, TimeSpan.FromSeconds(CachingConsts.OneDay));
    }
}
```
- 在xxxAppService.cs 注入CacheService。

```csharp
public class CacheService : AbstractCacheService
{
    private readonly Lazy<ICacheProvider> _cache;

    public CacheService(Lazy<ICacheProvider> cache
    , Lazy<IRedisProvider> redisProvider
    : base(cache, redisProvider, distributedLocker)
    {
    _cache = cache;
    }

    //缓存预热方法。项目在启动时，会调用该方法。
    public override async Task PreheatAsync()
    {
        //其他需要预热的缓存
        await GetAllDeptsFromCacheAsync();
        //其他需要预热的缓存
    }

    //布隆过滤器
    internal (IBloomFilter CacheKeys, IBloomFilter Accounts) BloomFilters
    {
        get
        {
            var cacheFilter = _bloomFilterFactory.Value.GetBloomFilter(_cache.Value.CacheOptions.PenetrationSetting.BloomFilterSetting.Name);
            var accountFilter = _bloomFilterFactory.Value.GetBloomFilter($"adnc:{nameof(BloomFilterAccount).ToLower()}");
            return (cacheFilter, accountFilter);
        }
    }
}
```
## Cache拦截器使用
拦截器要根据实际业务场景使用，拦截器解决不了所有问题。当拦截器解决不了时，则只能在业务逻辑中使用编码的方式使用cache。
#### CachingEvictAttribute 删除拦截器，在IxxxappService接口使用CachingEvictAttribute特性<br/>

| 参数           | 描述(以下参数可以组合使用)                                   |
| -------------- | ------------------------------------------------------------ |
| CacheKey       | 删除指定CacheKey                                             |
| CacheKeys      | 删除一组CacheKey                                             |
| CacheKeyPrefix | 删除包含前缀的一组CacheKey，需要配合参数特性CachingParamAttribute |

#### 实例代码
```csharp
public interface IUserAppService : IAppService
{
    [CachingEvict(
        CacheKeys = new[] { CachingConsts.MenuRelationCacheKey, CachingConsts.MenuCodesCacheKey 
        }
        , CacheKeyPrefix = CachingConsts.UserValidateInfoKeyPrefix)]
    Task<AppSrvResult> SetRoleAsync([CachingParam] long id,UserSetRoleDto input);  

    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidateInfoKeyPrefix)]
    Task<AppSrvResult> ChangeStatusAsync([CachingParam] IEnumerable<long> ids, int status);
}


```
#### CachingAbleAttribute 读取拦截器
| 参数           | 描述                                                         |
| -------------- | ------------------------------------------------------------ |
| CacheKey       | 读取指定CacheKey                                             |
| CacheKeyPrefix | 读取指定前缀的Cachkey，需要配合参数特性CachingParamAttribute |
| Expiration     | 过期时间                                                     |

#### 实例代码
```csharp
public interface IDeptAppService : IAppService
{
    [CachingAble(CacheKey = CachingConsts.DetpTreeListCacheKey, Expiration = CachingConsts.OneYear)]
    Task<List<DeptTreeDto>> GetTreeListAsync();
}

public interface IAccountAppService : IAppService
{
    [CachingAble(CacheKeyPrefix = CachingConsts.UserValidateInfoKeyPrefix)]
    Task<UserValidateDto> GetUserValidateInfoAsync([CachingParam] long id);
}
```

## Cachekey布隆过滤器的使用
`Adnc.Infra.Caching`防穿透的实现是通过布隆过滤器实现的。下面介绍如何在项目中定义和使用,我以Admin微服务为例。

- 第一步
在Adnc.Admin.Application工程的Caching目录中新建BloomFilterCacheKey.cs，并继承AbstractBloomFilter
- 第二步
覆写 InitAsync 方法，该方法负责初始化布隆过滤器，项目启动时，会自动调用该方法，将系统中使用到cachekey保存进过滤器中。
默认InitAsync只会执行一次，布隆过滤器创建成功后，项目再次启动也不会被调用，这里需要根据自己的实际情况调整。

```csharp
namespace Adnc.Usr.Application.Caching
{
    public class BloomFilterCacheKey : AbstractBloomFilter
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IDistributedLocker> _distributedLocker;
        private readonly Lazy<IServiceProvider> _services;

        public BloomFilterCacheKey(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker
            , Lazy<IServiceProvider> services)
            : base(cache, redisProvider, distributedLocker)
        {
            _cache = cache;
            _distributedLocker = distributedLocker;
            _services = services;

            //从配置自动获取过滤器配置(appsettings.xxx.json)
            var setting = cache.Value.CacheOptions.PenetrationSetting.BloomFilterSetting;
            Name = setting.Name;
            ErrorRate = setting.ErrorRate;
            Capacity = setting.Capacity;
        }

        //过滤器名字，对应redis中的key
        public override string Name { get; }
        //容错率
        public override double ErrorRate { get; }
        //容量大小
        public override int Capacity { get; }

        public override async Task InitAsync()
        {
            //调用基类方法，初始化过滤器
            await base.InitAsync(async () =>
            {

                var values = new List<string>()
                {
                    //这些都是固定的cachekey
                     CachingConsts.MenuListCacheKey
                    ,CachingConsts.MenuTreeListCacheKey
                    ,CachingConsts.MenuRelationCacheKey
                    ,CachingConsts.MenuCodesCacheKey
                    ,CachingConsts.DetpListCacheKey
                    ,CachingConsts.DetpTreeListCacheKey
                    ,CachingConsts.DetpSimpleTreeListCacheKey
                    ,CachingConsts.RoleListCacheKey
                };

                var ids = new List<long>();
                using (var scope = _services.Value.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
                    ids = await repository.GetAll().Select(x => x.Id).ToListAsync();
                }

                //这里不是固定的cachekey,只是前缀一样。
                //public const string UserValidateInfoKeyPrefix = "adnc:usr:users:validateinfo";
                //如账号alpha2008登录后，它的状态信息会保存在cache中，cachekey为adnc:usr:users:validateinfo:160000000000
                if (ids?.Any() == true)
                    values.AddRange(ids.Select(x => string.Concat(CachingConsts.UserValidateInfoKeyPrefix, CachingConsts.LinkChar, x)));

                return values;
            });
        }
    }
}

```

- 第三步 在cacheservice.cs中组合BloomFilterCacheKey（主要是为了方便调用）。

```csharp
namespace Adnc.Usr.Application.Caching
{
    public class CacheService : AbstractCacheService
    {
        private readonly Lazy<IBloomFilterFactory> _bloomFilterFactory;
        public CacheService(Lazy<IBloomFilterFactory> bloomFilterFactory)
        {
            _bloomFilterFactory = bloomFilterFactory;
        }
        public override async Task PreheatAsync(){};

        internal (IBloomFilter CacheKeys, IBloomFilter Accounts) BloomFilters
        {
            get
            {
                //cachekey布隆过滤器
                var cacheFilter = _bloomFilterFactory.Value.GetBloomFilter(_cache.Value.CacheOptions.PenetrationSetting.BloomFilterSetting.Name);
                //账号布隆过滤器，这是另外一个过滤器，和cache没有关系。
                var accountFilter = _bloomFilterFactory.Value.GetBloomFilter($"adnc:{nameof(BloomFilterAccount).ToLower()}");
                return (cacheFilter, accountFilter);
            }
        }
    }
}
```

- 第四步
动态添加cachekey到BloomFilterCacheKey过滤器
```csharp
public class UserAppService : AbstractAppService, IUserAppService
{
    private readonly IEfRepository<SysUser> _userRepository;
    private readonly CacheService _cacheService;

    public async Task<AppSrvResult<long>> CreateAsync(UserCreationDto input)
    {
        //其他业务逻辑
        user.Id = IdGenerater.GetNextId();

        var cacheKey = _cacheService.ConcatCacheKey(CachingConsts.UserValidateInfoKeyPrefix, user.Id);
        //添加到BloomFiltersCacheKey过滤器
        await _cacheService.BloomFilters.CacheKeys.AddAsync(cacheKey);
        
        await _userRepository.InsertAsync(user);
        //其他业务逻辑
    }
}
```

## 其它布隆过滤器
在上一节中我们看到还有一个accountFilter过滤器（BloomFilterAccount）,这个过滤器和cache没有关系。具体实现请参考源码，所有布隆过滤器的定义都一样。
BloomFilterAccount用于登录时判断account是否存在。

```csharp
namespace Adnc.Usr.Application.Services
{
    public class AccountAppService : AbstractAppService, IAccountAppService
    {
        private readonly CacheService _cacheService;

        public AccountAppService(CacheService cacheService)
        {
            _cacheService = cacheService;
        }
        
        public async Task<AppSrvResult<UserValidateDto>> LoginAsync(UserLoginDto inputDto)
        {
            //这里用到了BloomFilterAccount这个过滤器
            //LoginAsync这个方法是Adnc目前唯一不需要登录就能访问的方法，当然正常情况下，直接通过查询数据库判断是没有问题的
            //如果一些别有用心的人，模拟数千并发，来调用这个方法，目前的数据库配置肯定扛不住。
            //如果先通过布隆过滤器处理，数千并发过来，对redis来说都没有太多压力。
            var exists = await _cacheService.BloomFilters.Accounts.ExistsAsync(inputDto.Account.ToLower());
            if(!exists)
                return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

            var user = await _userRepository.FetchAsync(x => x.Account == inputDto.Account);
            if (user == null)
                return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");
        }
    }
}
```

## 布隆过滤器其他应用场景
- 用户注册，实时判断是否存在同名账户。
- 重复数据判断。

以上两种业务场景，本人都用到布隆过滤器来处理。

## 探讨更优雅的实现方式
- 数据库与缓存同步
- 布隆过滤器实时添加数据
- 数据库与ES同步
- 数据库与MQ同步。

如果您的业务场景需要实现上面的这些功能，阿里开源的canal或许是比较好的选择。使用canal能优雅更方便的组织业务代码。

---
—— 完 ——

如有帮助，欢迎 [star & fork](https://github.com/alphayu/adnc)。
