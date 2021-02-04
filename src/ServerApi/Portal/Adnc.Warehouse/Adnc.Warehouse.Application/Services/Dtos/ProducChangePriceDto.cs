using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Warehouse.Application.Dtos
{
    public class ProducChangePriceDto : BaseDto
    {
        public float Price { set; get; }
    }
}
