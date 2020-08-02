using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    /// <summary>
    /// 角色检索条件
    /// </summary>
    public class DictSearchDto : BaseSearchDto
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
