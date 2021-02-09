using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Maint.Application.Dtos
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Serializable]
    public class CfgDto : OutputFullAuditInfoDto<long>
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }
    }
}
