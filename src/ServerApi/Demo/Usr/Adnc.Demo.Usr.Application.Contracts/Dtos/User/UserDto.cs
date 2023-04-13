namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class UserDto : OutputBaseAuditDto
    {
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public long? DeptId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; } = string.Empty;

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 角色Id列表，以逗号分隔
        /// </summary>
        public string RoleIds { get; set; } = string.Empty;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleNames { get; set; } = string.Empty;

        /// <summary>
        /// 性别
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// 性别描述
        /// </summary>
        public string SexName
        {
            get
            {
                string result = "未知";
                if (Sex.HasValue)
                {
                    result = Sex.Value == 1 ? "男" : "女";
                }

                return result;
            }
        }

        /// <summary>
        /// 账户状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 账户状态描述
        /// </summary>
        public string StatusName => Status == 1 ? "启用" : "禁用";
    }
}