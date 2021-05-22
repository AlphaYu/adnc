using System;
using Yitter.IdGenerator;

namespace Adnc.Infra.Common.Helper.IdGeneraterInternal
{
    public class YitterSnowFlake
    {
        private static readonly Lazy<YitterSnowFlake> lazy = new Lazy<YitterSnowFlake>(() => new YitterSnowFlake());
        public static byte YitterWorkerIdBitLength { get { return 6; } }
        public static byte YitterSeqBitLength { get { return 6; } }
        public static ushort MaxWorkerId { get { return (ushort)(Math.Pow(2.0, YitterWorkerIdBitLength) - 1); } }

        private static short _currentWorkerId = -1;

        public static short CurrentWorkerId
        {
            get { return _currentWorkerId; }
            set
            {
                if (value < 0 || value > MaxWorkerId)
                    throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0", MaxWorkerId));
                if (_currentWorkerId > -1)
                    throw new Exception(string.Format("worker Id can't be changed.{0}", _currentWorkerId));
                else
                    _currentWorkerId = value;
            }
        }

        static YitterSnowFlake()
        {
        }

        private YitterSnowFlake()
        {
            if (_currentWorkerId > MaxWorkerId || _currentWorkerId < 0)
                throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0", MaxWorkerId));

            YitIdHelper.SetIdGenerator(new IdGeneratorOptions((ushort)_currentWorkerId)
            {
                WorkerIdBitLength = YitterWorkerIdBitLength
                ,
                SeqBitLength = YitterSeqBitLength
            }); ;
        }

        public static YitterSnowFlake Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public long NextId()
        {
            return YitIdHelper.NextId();
        }
    }
}