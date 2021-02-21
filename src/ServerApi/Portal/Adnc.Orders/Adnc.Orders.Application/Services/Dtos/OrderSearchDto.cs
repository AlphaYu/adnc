using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Orders.Application.Dtos
{
    public class OrderSearchDto : SearchPagedDto
    {
        public long? Id { get; set; }
    }
}
