using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDto : BaseDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 基本信息
        /// </summary>
        public UserProfileDto Profile { get; set; }

        /// <summary>
        /// 角色集合
        /// </summary>
        public List<string> Roles { get; private set; } = new List<string>();

        /// <summary>
        /// 权限集合
        /// </summary>
        public List<string> Permissions { get; private set; } = new List<string>();
    }
}
