using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    /// <summary>
    /// 角色检索条件
    /// </summary>
    public class RolePagedSearchDto : SearchPagedDto
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
    }
}
