using Adnc.Application.Shared.Dtos;

namespace Adnc.Usr.Application.Dtos
{
    public class RolePermissionsCheckInputDto : BaseDto
    {
        public long[] RoleIds { get; set; }
        public string[] Permissions { get; set; }

    }
}
