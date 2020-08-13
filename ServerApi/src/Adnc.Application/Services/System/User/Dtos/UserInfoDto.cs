using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDto : BaseDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户个人信息
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
