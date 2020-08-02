using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Application.Dtos;

namespace Adnc.Application.Dtos
{
    public class DictSaveInputDto : BaseDto<long>
    {
        public string DictName { get; set; }

        public string Tips { get; set; }

        public string DictValues { get; set; }
    }
}
