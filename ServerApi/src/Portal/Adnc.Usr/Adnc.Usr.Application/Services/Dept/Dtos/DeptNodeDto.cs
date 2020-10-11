using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    /// <summary>
    /// 部门节点
    /// </summary>
    [Serializable]
    public class DeptNodeDto : DeptDto
    {
        /// <summary>
        /// 子部门
        /// </summary>
        public List<DeptNodeDto> Children { get; private set; } = new List<DeptNodeDto>();
    }
}
