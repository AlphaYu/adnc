using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class RoleSetInputDto : BaseInputDto
    {
        public long[] RoleIds { get; set; }
    }
}
