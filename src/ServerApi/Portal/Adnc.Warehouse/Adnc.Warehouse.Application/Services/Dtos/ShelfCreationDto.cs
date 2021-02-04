using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Warehouse.Application.Dtos
{
    public class ShelfCreationDto : BaseDto
    {
        public string PositionCode { get; set; }

        public string PositionDescription { get; set; }
    }
}
