namespace Adnc.Demo.Admin.Application.Contracts.Dtos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDto : IDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///  用户名
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        ///  姓名/昵称
        /// </summary>
        public string Name { get; set; } = string.Empty;

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

        /// <summary>
        /// 角色代码集合
        /// </summary>
        public string[] Roles { get; set; } = [];

        /// <summary>
        /// 权限集合
        /// </summary>
        public string[] Perms { get; set; } = [];
    }
}