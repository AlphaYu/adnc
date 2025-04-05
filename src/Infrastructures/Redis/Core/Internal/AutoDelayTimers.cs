namespace Adnc.Infra.Redis.Core.Internal;

public class AutoDelayTimers
{
    private static readonly Lazy<AutoDelayTimers> _lazy = new(() => new AutoDelayTimers());
    private static readonly ConcurrentDictionary<string, Timer> _timers = new();

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
            return _lazy.Value;
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
            if (_timers.TryRemove(key, out var timer))
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
