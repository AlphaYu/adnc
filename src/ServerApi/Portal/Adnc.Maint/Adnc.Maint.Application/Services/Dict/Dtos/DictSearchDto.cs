using Adnc.Application.Shared.Dtos;

namespace Adnc.Maint.Application.Dtos
{
    /// <summary>
    /// 角色检索条件
    /// </summary>
    public class DictSearchDto : SearchDto
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
