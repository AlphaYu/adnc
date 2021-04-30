using System;
using Yitter.IdGenerator;

namespace Adnc.Infra.Common.Helper.IdGeneraterInternal
{
    public class YitterSnowFlake
    {
        private static readonly YitterSnowFlake _instance = new YitterSnowFlake();
        private const byte YitterWorkerIdBitLength = 6;
        private const byte YitterSeqBitLength = 6;

        static YitterSnowFlake() { }

        private YitterSnowFlake()
        {
            //这种方式，每秒并发超10W,如果一个服务部署10个实例，Id存在1/10000 概率重复。
            var workIdStr = Environment.GetEnvironmentVariable("WorkerId");
            ushort.TryParse(workIdStr, out ushort workerId);

            var maxWorkerId = (ushort)Math.Pow(2.0, YitterWorkerIdBitLength);
            workerId = 1;
            if (workerId > maxWorkerId || workerId < 1)
                throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0", maxWorkerId));

            YitIdHelper.SetIdGenerator(new IdGeneratorOptions(workerId)
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
                return _instance;
            }
        }

        public long NextId()
        {
            return YitIdHelper.NextId();
        }
    }
}
