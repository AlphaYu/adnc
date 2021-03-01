using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Ord.Application.Dtos
{
    public class OrderSearchPagedDto : SearchPagedDto
    {
        public long? Id { get; set; }
    }
}
