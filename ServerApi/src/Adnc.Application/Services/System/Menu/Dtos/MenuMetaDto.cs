using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    /// <summary>
    /// 菜单元数据
    /// </summary>
    [Serializable]
    public class MenuMetaDto
    {
        /// <summary>
        /// 菜单标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
    }
}
