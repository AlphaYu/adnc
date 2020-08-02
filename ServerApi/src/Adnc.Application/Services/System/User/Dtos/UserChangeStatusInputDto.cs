using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class UserChangeStatusInputDto
    {
        public long[] UserIds { get; set; }

        public int Status { get; set; }
    }
}
