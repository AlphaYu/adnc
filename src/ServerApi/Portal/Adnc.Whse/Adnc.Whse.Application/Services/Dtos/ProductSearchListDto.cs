﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Dtos
{
    public class ProductSearchListDto : SearchDto
    {
        public string[] Ids { get; set; }

        public int StatusCode { get; set; }
    }
}
