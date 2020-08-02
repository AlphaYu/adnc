using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class RolePermissionsCheckInputDto
    {
        [Required]
        public long[] RoleIds { get; set; }
        [Required]
        public string[] Permissions { get; set; }

    }
}
