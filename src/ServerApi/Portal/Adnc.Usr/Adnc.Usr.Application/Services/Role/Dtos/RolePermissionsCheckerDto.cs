using Adnc.Application.Shared.Dtos;

namespace Adnc.Usr.Application.Dtos
{
    public class RolePermissionsCheckerDto : IDto
    {
        public long[] RoleIds { get; set; }
        public string[] Permissions { get; set; }

    }
}
