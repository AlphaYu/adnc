namespace Adnc.Infra.Redis.Caching.Configurations
{
    /// <summary>
    /// redis options.
    /// </summary>
    public sealed class CacheOptions
    {
        /// <summary>
        /// Gets or sets the max random second.
        /// </summary>
        /// <remarks>
        /// If this value greater then zero, the seted cache items' expiration will add a random second
        /// This is mainly for preventing Cache Crash
        /// </remarks>
        /// <value>The max random second.</value>
        public int MaxRdSecond { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether enable logging.
        /// </summary>
        /// <value><c>true</c> if enable logging; otherwise, <c>false</c>.</value>
        public bool EnableLogging { get; set; }

        /// <summary>
        /// Gets or sets the sleep ms.
        /// when mutex key alive, it will sleep some time, default is 300
        /// </summary>
        /// <value>The sleep ms.</value>
        public int SleepMs { get; set; } = 300;

        /// <summary>
        /// Gets or sets the lock ms.
        /// mutex key's alive time(ms), default is 5000
        /// </summary>
        /// <value>The lock ms.</value>
        public int LockMs { get; set; } = 5000;

        /// <summary>
        ///Penetration setting
        /// </summary>
        public PenetrationOptions PenetrationSetting { get; set; } = default!;

        /// <summary>
        /// polly timeout seconds
        /// </summary>
        public int PollyTimeoutSeconds { get; set; } = 11;

        public sealed class PenetrationOptions
        {
            public bool Disable { get; set; }

            public BloomFilterSetting BloomFilterSetting { get; set; } = default!;
        }

        public sealed class BloomFilterSetting
        {
            public string Name { get; set; } = string.Empty;

            public int Capacity { get; set; }

            public double ErrorRate { get; set; }
        }
    }
}