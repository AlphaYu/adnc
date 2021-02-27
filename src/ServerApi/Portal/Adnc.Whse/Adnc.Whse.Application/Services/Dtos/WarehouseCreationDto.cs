using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Dtos
{
    public class WarehouseCreationDto : IDto
    {
        public string PositionCode { get; set; }

        public string PositionDescription { get; set; }
    }
}
