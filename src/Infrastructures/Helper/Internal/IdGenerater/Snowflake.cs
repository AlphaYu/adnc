using System.Text;

namespace Adnc.Infra.Helper.Internal.IdGenerater;

/// <summary>
/// Snowflake ID
/// Twitter_Snowflake
/// SnowFlake structure (each part separated by -)
/// 0 - 0000000000 0000000000 0000000000 0000000000 0 - 00000 - 00000 - 000000000000
/// 1-bit sign: since long in Java is signed, the MSB is the sign bit (0 = positive, 1 = negative), so IDs are always positive.
/// 41-bit timestamp (milliseconds): stores the difference between the current time and the start time.
/// 41 bits allow usage for 69 years: T = (1L << 41) / (1000L * 60 * 60 * 24 * 365) = 69.
/// The start timestamp is typically the time when the ID generator was first used, specified by the program (e.g., the startTime property of IdWorker).
/// 10-bit machine bits: supports deployment on 1024 nodes, including 5-bit datacenterId and 5-bit workerId.
/// 12-bit sequence: intra-millisecond counter; supports 4096 IDs per node per millisecond (same machine, same timestamp).
/// Total: exactly 64 bits, a Long type.
/// SnowFlake advantages: globally time-ordered, no ID collisions across distributed systems (distinguished by data center ID and machine ID),
/// and highly efficient — tested to produce up to 4,096,000 IDs per second on a single machine.
/// </summary>
[Obsolete("Native Snowflake algorithm, now deprecated")]
internal class Snowflake
{
    // Start timestamp (new DateTime(2020, 1, 1).ToUniversalTime() - Jan1st1970).TotalMilliseconds
    private const long twepoch = 1577808000000L;

    // Number of bits for worker ID
    private const int workerIdBits = 5;

    // Number of bits for data center ID
    private const int datacenterIdBits = 5;

    // Max worker ID = 31 (this bit-shift quickly calculates the max decimal value representable by N binary bits)
    private const long maxWorkerId = -1L ^ (-1L << workerIdBits);

    // Max data center ID = 31
    private const long maxDatacenterId = -1L ^ (-1L << datacenterIdBits);

    // Number of bits for sequence
    private const int sequenceBits = 12;

    // Data center ID left-shifted by 17 bits (12+5)
    private const int datacenterIdShift = sequenceBits + workerIdBits;

    // Worker ID left-shifted by 12 bits
    private const int workerIdShift = sequenceBits;

    // Timestamp left-shifted by 22 bits (5+5+12)
    private const int timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;

    // Sequence mask = 4095 (0b111111111111=0xfff=4095)
    private const long sequenceMask = -1L ^ (-1L << sequenceBits);

    // Data center ID (0~31)
    public long datacenterId { get; private set; }

    // Worker machine ID (0~31)
    public long workerId { get; private set; }

    // Intra-millisecond sequence (0~4095)
    public long sequence { get; private set; }

    // Timestamp of last generated ID
    public long lastTimestamp { get; private set; }

    private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static readonly object _syncRoot = new object(); // Lock object

    private static Snowflake _snowflake;

    /// <summary>
    /// Creates an instance.
    /// </summary>
    /// <returns></returns>
    public static Snowflake GetInstance(long datacenterId = 1, long workerId = 1)
    {
        if (_snowflake == null)
        {
            lock (_syncRoot)
            {
                _snowflake ??= new Snowflake(datacenterId, workerId);
            }
        }
        else
        {
            if (_snowflake.datacenterId != datacenterId || _snowflake.workerId != workerId)
                throw new Exception($"Original datacenterId:{_snowflake.datacenterId},workerId={_snowflake.workerId}");
        }
        return _snowflake;
    }

    /// <summary>
    /// Snowflake ID
    /// </summary>
    /// <param name="datacenterId">Data center ID</param>
    /// <param name="workerId">Worker machine ID</param>
    private Snowflake(long datacenterId, long workerId)
    {
        if (datacenterId > maxDatacenterId || datacenterId < 0)
        {
            throw new Exception(string.Format("datacenter Id can't be greater than {0} or less than 0", maxDatacenterId));
        }
        if (workerId > maxWorkerId || workerId < 0)
        {
            throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0", maxWorkerId));
        }
        this.workerId = workerId;
        this.datacenterId = datacenterId;
        this.sequence = 0L;
        this.lastTimestamp = -1L;
    }

    /// <summary>
    /// Gets the next ID.
    /// </summary>
    /// <returns></returns>
    public long NextId()
    {
        lock (_syncRoot)
        {
            long timestamp = GetCurrentTimestamp();
            if (timestamp > lastTimestamp) // Timestamp changed; reset intra-millisecond sequence
            {
                sequence = 0L;
            }
            else if (timestamp == lastTimestamp) // Same millisecond; increment intra-millisecond sequence
            {
                sequence = (sequence + 1) & sequenceMask;
                if (sequence == 0) // Intra-millisecond sequence overflow
                {
                    timestamp = GetNextTimestamp(lastTimestamp); // Block until next millisecond to get a new timestamp
                }
            }
            else // Current time is less than last ID timestamp; clock was rolled back, handle accordingly
            {
                sequence = (sequence + 1) & sequenceMask;
                if (sequence > 0)
                {
                    timestamp = lastTimestamp;     // Stay at last timestamp; wait for system clock to catch up, resolving the rollback
                }
                else   // Intra-millisecond sequence overflow
                {
                    timestamp = lastTimestamp + 1;   // Advance directly to next millisecond
                }
                //throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", lastTimestamp - timestamp));
            }

            lastTimestamp = timestamp;       // Update timestamp of last generated ID

            // Combine via bit-shifting and OR to form the 64-bit ID
            var id = ((timestamp - twepoch) << timestampLeftShift)
                    | (datacenterId << datacenterIdShift)
                    | (workerId << workerIdShift)
                    | sequence;
            return id;
        }
    }

    /// <summary>
    /// Blocks until the next millisecond to obtain a new timestamp.
    /// </summary>
    /// <param name="lastTimestamp">Timestamp of last generated ID</param>
    /// <returns>Current timestamp</returns>
    private long GetNextTimestamp(long lastTimestamp)
    {
        long timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }

    /// <summary>
    /// Gets the current timestamp.
    /// </summary>
    /// <returns></returns>
    private long GetCurrentTimestamp()
    {
        return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
    }

    /// <summary>
    /// Parses a Snowflake ID.
    /// </summary>
    /// <returns></returns>
    public static string AnalyzeId(long Id)
    {
        StringBuilder sb = new StringBuilder();

        var timestamp = (Id >> timestampLeftShift);
        var time = Jan1st1970.AddMilliseconds(timestamp + twepoch);
        sb.Append(time.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss:fff"));

        var datacenterId = (Id ^ (timestamp << timestampLeftShift)) >> datacenterIdShift;
        sb.Append("_" + datacenterId);

        var workerId = (Id ^ ((timestamp << timestampLeftShift) | (datacenterId << datacenterIdShift))) >> workerIdShift;
        sb.Append("_" + workerId);

        var sequence = Id & sequenceMask;
        sb.Append("_" + sequence);

        return sb.ToString();
    }
}
