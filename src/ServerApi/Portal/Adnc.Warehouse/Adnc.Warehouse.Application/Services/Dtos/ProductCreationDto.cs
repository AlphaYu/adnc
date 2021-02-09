using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Warehouse.Application.Dtos
{
    public class ProductCreationDto : IDto
    {
        public string Sku { set; get; }

        public string Name { set; get; }

        public string Describe { set; get; }

        public float Price { set; get; }

        public string Unit { set; get; }
    }
}
