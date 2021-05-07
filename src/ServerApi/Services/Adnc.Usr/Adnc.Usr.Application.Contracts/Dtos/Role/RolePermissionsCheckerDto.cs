using Adnc.Application.Shared.Dtos;
using System.Collections.Generic;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    public class RolePermissionsCheckerDto : IDto
    {
        public IEnumerable<long> RoleIds { get; set; }
        public IEnumerable<string> Permissions { get; set; }

    }
}
