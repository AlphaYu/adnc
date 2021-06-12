using Adnc.Application.Shared.Dtos;

namespace Adnc.Maint.Application.Contracts.Dtos
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class CfgCreationDto : IInputDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }
    }
}