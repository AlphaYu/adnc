namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDto : IDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///  用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        ///  昵称
        /// </summary>
        public string NickName { get; set; } = string.Empty;

        /// <summary>
        ///  头像
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

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