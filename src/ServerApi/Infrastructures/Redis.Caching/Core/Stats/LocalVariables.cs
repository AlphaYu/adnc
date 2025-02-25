namespace Adnc.Infra.Redis.Caching.Core
{
    public sealed class LocalVariables
    {
        private static readonly Lazy<LocalVariables> lazy = new(() => new LocalVariables());
        private static readonly ConcurrentQueue<Model> _queue = new();

        static LocalVariables()
        {
        }

        private LocalVariables()
        {
        }

        public static LocalVariables Instance => lazy.Value;

        public ConcurrentQueue<Model> Queue => _queue;

        public sealed class Model
        {
            public List<string> CacheKeys { get; init; }
            public DateTime ExpireDt { get; init; }

            public Model(IEnumerable<string> cacheKeys, DateTime expireDt)
            {
                CacheKeys = cacheKeys.ToList();
                ExpireDt = expireDt;
            }
        }
    }
}