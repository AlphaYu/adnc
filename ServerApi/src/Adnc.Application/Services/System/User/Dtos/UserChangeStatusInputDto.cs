using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class UserChangeStatusInputDto : BaseDto
    {
        public long[] UserIds { get; set; }

        public int Status { get; set; }
    }
}
