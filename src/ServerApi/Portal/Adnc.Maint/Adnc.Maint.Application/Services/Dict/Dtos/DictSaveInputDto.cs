using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Dtos;

namespace  Adnc.Maint.Application.Dtos
{
    public class DictSaveInputDto : BaseInputDto
    {
        public string DictName { get; set; }

        public string Tips { get; set; }

        public string DictValues { get; set; }
    }
}
