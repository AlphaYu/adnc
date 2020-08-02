using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class PermissonSaveInputDto
    {
        public long RoleId { set; get; } 
        public long[] Permissions { get; set; }
    }
}
