using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Cus.Application.Dtos
{
    public class CustomerRegisterDto : IInputDto
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string Nickname { get; set; }

        public string Realname { get; set; }
    }
}
