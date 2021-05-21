using Adnc.Infra.Common.Helper.IdGeneraterInternal;

namespace Adnc.Infra.Common.Helper
{
    public static class IdGenerater
    {
        /// <summary>
        /// 获取唯一Id，默认支持64个节点，每毫秒下的序列数6位。
        /// 在默认配置下，ID可用 71000 年不重复
        /// 在默认配置下，70年才到 js Number Max 值
        /// 默认情况下，500毫秒可以生成10W个号码。
        /// 如果需要提高速度，可以修改SeqBitLength长度。当SeqBitLength =10 ,100W个id约800毫秒。
        /// </summary>
        /// <returns></returns>
        public static long GetNextId()
        {
            return YitterSnowFlake.Instance.NextId();
        }
    }
}