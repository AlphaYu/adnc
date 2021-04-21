using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    public class UserSetRoleDto : IInputDto
    {
        public long[] RoleIds { get; set; }
    }
}
