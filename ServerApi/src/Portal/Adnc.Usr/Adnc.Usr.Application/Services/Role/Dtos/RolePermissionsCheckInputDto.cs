using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    public class RolePermissionsCheckInputDto : BaseDto
    {
        [Required]
        public long[] RoleIds { get; set; }
        [Required]
        public string[] Permissions { get; set; }

    }
}
