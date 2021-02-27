using Adnc.Infr.Common.Exceptions;

namespace Adnc.Core.Shared.Domain.Entities
{
    public static class AggregateRootExtension
    {
        /// <summary>
        /// 检查是否是一个正常的实体
        /// </summary>
        /// <param name="value"></param>
        public static void CheckIsNormal(this AggregateRoot value)
        {
            if (value == null)
                throw new AdncArgumentNullException(nameof(value));
            if (value.Id <= 0)
                throw new AdncArgumentException(nameof(value));
        }
    }
}
