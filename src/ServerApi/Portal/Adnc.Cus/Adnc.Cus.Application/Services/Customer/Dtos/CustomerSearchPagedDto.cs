using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Cus.Application.Dtos
{
    public class CustomerSearchPagedDto: SearchPagedDto
    {
        public string Id { get; set; }

        public string Account { get; set; }
    }
}
