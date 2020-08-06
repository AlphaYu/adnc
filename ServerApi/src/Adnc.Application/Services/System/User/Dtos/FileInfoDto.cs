using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class FileInfoDto : BaseDto
    {
        public string OriginalFileName { get; set; }

        public string RealFileName { get; set; }
    }
}
