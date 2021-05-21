using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.Infra.Caching.Core
{
    public sealed class LocalVariables
    {
        private static readonly Lazy<LocalVariables> lazy = new Lazy<LocalVariables>(() => new LocalVariables());
        private static ConcurrentQueue<Model> _queue;

        static LocalVariables()
        {
        }

        private LocalVariables()
        {
            _queue = new ConcurrentQueue<Model>();
        }

        public static LocalVariables Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public ConcurrentQueue<Model> Queue => _queue;

        public sealed class Model
        {
            public List<string> CacheKeys { get; }
            public DateTime ExpireDt { get; }

            public Model(IEnumerable<string> cacheKeys, DateTime expireDt)
            {
                CacheKeys = cacheKeys.ToList();
                ExpireDt = expireDt;
            }
        }
    }
}