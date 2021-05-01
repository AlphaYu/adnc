using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 精简部门树结构
    /// </summary>
    [Serializable]
    public class DeptSimpleTreeDto : OutputDto<long>
    {
        /// <summary>
        /// 部门简称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 子部门
        /// </summary>
        public List<DeptSimpleTreeDto> children { get;  set; } = new List<DeptSimpleTreeDto>();
    }
}
