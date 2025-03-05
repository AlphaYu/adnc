namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    public class UserProfileDto : IDto
    {
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptFullName { get; set; }

        /// <summary>
        /// 电邮
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleNames { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
    }
}