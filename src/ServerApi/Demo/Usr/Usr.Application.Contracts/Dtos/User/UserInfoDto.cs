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