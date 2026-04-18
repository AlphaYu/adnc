using Adnc.Infra.Core.Guard;
using Yitter.IdGenerator;

namespace Adnc.Infra.IdGenerater.Yitter;

public static class IdGenerater
{
    private static bool _isSet;
    private static readonly object _locker = new();

    public static byte WorkerIdBitLength => 6;
    public static byte SeqBitLength => 6;
    public static short MaxWorkerId => (short)(Math.Pow(2.0, WorkerIdBitLength) - 1);
    public static short CurrentWorkerId { get; private set; } = -1;

    /// <summary>
    /// Initializes the ID generator.
    /// </summary>
    /// <param name="workerId"></param>
    public static void SetWorkerId(ushort workerId)
    {
        if (_isSet)
        {
            throw new InvalidOperationException("allow only once");
        }

        if (workerId > MaxWorkerId || workerId < 0)
        {
            throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId} or less than 0");
        }

        lock (_locker)
        {
            if (_isSet)
            {
                throw new InvalidOperationException("allow only once");
            }

            YitIdHelper.SetIdGenerator(new IdGeneratorOptions(workerId)
            {
                WorkerIdBitLength = WorkerIdBitLength,
                SeqBitLength = SeqBitLength
            });

            CurrentWorkerId = (short)workerId;
            _isSet = true;
        }
    }

    /// <summary>
    /// Gets a unique ID. By default, 64 nodes are supported with a 6-bit sequence per millisecond.
    /// With default settings, IDs remain unique for 71,000 years.
    /// With default settings, it takes 70 years to reach the JavaScript Number.MAX_SAFE_INTEGER.
    /// By default, 100,000 IDs can be generated in 500 milliseconds.
    /// To increase throughput, increase SeqBitLength. With SeqBitLength=10, ~1,000,000 IDs take ~800ms.
    /// </summary>
    /// <returns>Id</returns>
    public static long GetNextId()
    {
        if (!_isSet)
        {
            throw new InvalidOperationException("please call SetIdGenerator first");
        }

        return YitIdHelper.NextId();
    }

    public static long[] GetNextIds(int number)
    {
        Checker.Argument.ThrowIfOutOfRange(number, 1, 100000, nameof(number));

        var ids = new long[number];
        for (var index = 0; index < number; index++)
        {
            ids[index] = YitIdHelper.NextId();
        }
        return ids;
    }

    public static string GetNextIdString(string prefix)
    {
        return prefix.Trim().ToUpper() + GetNextId().ToString();
    }
}
