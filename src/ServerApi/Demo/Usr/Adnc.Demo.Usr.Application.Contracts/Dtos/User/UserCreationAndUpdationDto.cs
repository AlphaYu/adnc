namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    public abstract class UserCreationAndUpdationDto : InputDto
    {
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; } = string.Empty;

        ///// <summary>
        ///// 头像
        ///// </summary>
        ////public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public long? DeptId { get; set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        ///// <summary>
        ///// 角色Id列表，以逗号分隔
        ///// </summary>
        ////public string RoleId { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 账户状态
        /// </summary>
        public int Status { get; set; }

    }
}