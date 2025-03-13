namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    public class UserProfileDto : UserDto
    {
        /// <summary>
        /// 多个角色名称
        /// </summary>
        public string RoleNames { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        private string avatar = string.Empty;
        public string Avatar
        {
            set { avatar = value; }
            get
            {
                if (avatar.IsNullOrEmpty())
                {
                    avatar = "https://foruda.gitee.com/images/1723603502796844527/03cdca2a_716974.gif";
                }
                return avatar;
            }
        }
    }

    public class UserProfileUpdationDto : InputDto
    {
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 姓名/昵称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    public class UserProfileChangePwdDto : InputDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// 当前密码
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}