﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Dtos
{
    public class ProducChangePriceDto : IDto
    {
        public decimal Price { set; get; }
    }
}
