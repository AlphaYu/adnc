namespace Adnc.Infra.Redis.Caching.Core;

public sealed class LocalVariables
{
    private static readonly Lazy<LocalVariables> _lazy = new(() => new LocalVariables());
    private static readonly ConcurrentQueue<Model> _queue = new();

    static LocalVariables()
    {
    }

    private LocalVariables()
    {
    }

    public static LocalVariables Instance => _lazy.Value;

    public ConcurrentQueue<Model> Queue => _queue;

    public sealed class Model(IEnumerable<string> cacheKeys, DateTime expireDt)
    {
        public List<string> CacheKeys { get; init; } = cacheKeys.ToList();
        public DateTime ExpireDt { get; init; } = expireDt;
    }
}
