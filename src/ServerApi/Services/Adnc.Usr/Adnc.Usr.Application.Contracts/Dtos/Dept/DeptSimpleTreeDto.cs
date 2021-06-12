using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 精简部门树结构
    /// </summary>
    [Serializable]
    public class DeptSimpleTreeDto : OutputDto
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public override long Id { get; set; }

        /// <summary>
        /// 部门简称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 子部门
        /// </summary>
        public List<DeptSimpleTreeDto> Children { get; set; } = new List<DeptSimpleTreeDto>();
    }
}