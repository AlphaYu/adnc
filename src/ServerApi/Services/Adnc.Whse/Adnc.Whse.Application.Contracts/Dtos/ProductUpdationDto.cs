using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class ProductUpdationDto : IDto
    {
        public string Sku { set; get; }

        public string Name { set; get; }

        public string Describe { set; get; }

        public string Unit { set; get; }

        public decimal Price { set; get; }
    }
}
