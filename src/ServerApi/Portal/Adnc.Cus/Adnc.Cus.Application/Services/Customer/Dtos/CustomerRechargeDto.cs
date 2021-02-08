using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Cus.Application.Dtos
{
    public class CustomerRechargeDto : IInputDto
    {
        [Required]
        public long ID { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
