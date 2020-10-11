using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    public class RoleSetInputDto : BaseInputDto
    {
        public long[] RoleIds { get; set; }
    }
}
