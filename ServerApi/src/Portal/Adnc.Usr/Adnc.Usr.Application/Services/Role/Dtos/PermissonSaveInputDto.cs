using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    public class PermissonSaveInputDto : BaseDto
    {
        public long RoleId { set; get; } 
        public long[] Permissions { get; set; }
    }
}
