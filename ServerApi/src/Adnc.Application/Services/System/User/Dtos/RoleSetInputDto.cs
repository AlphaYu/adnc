using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class RoleSetInputDto : BaseInputDto<long>
    {
        public string RoleIds { get; set; }
    }
}
