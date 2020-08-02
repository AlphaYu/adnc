using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Services
{
    /// <summary>
    /// 实体管理状态
    /// </summary>
    public enum ManageStatus
    {
        /// <summary>
        /// 启用
        /// </summary>
        Enabled = 1,

        /// <summary>
        /// 禁用
        /// </summary>
        Disabled = 2,

        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 3
    }
}
