using Adnc.Application.Shared.Dtos;

namespace Adnc.Usr.Application.Dtos
{
    public class RoleSaveInputDto : BaseInputDto
    {
		/// <summary>
		/// 角色名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 角色描述
		/// </summary>
		public string Tips { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		public int Num { get; set; }

	}
}
