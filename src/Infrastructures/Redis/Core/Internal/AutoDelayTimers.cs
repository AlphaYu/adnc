namespace Adnc.Infra.Redis.Core.Internal
{
    public class AutoDelayTimers
    {
        private static readonly Lazy<AutoDelayTimers> lazy = new Lazy<AutoDelayTimers>(() => new AutoDelayTimers());
        private static ConcurrentDictionary<string, Timer> _timers = new ConcurrentDictionary<string, Timer>();

        static AutoDelayTimers()
        {
        }

        private AutoDelayTimers()
        {
        }

        public static AutoDelayTimers Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public bool TryAdd(string key, Timer dealytimer)
        {
            return _timers.TryAdd(key, dealytimer);
        }

        public void CloseTimer(string key)
        {
            if (_timers.ContainsKey(key))
            {
                if (_timers.TryRemove(key, out Timer? timer))
                {
                    timer?.Dispose();
                }
            }
        }

        public bool ContainsKey(string key)
        {
            return _timers.ContainsKey(key);
        }
    }
}