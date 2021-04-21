using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Shared.Dtos
{
    public abstract class MongoDto : IDto
    {
        public string Id { get; set; }
    }
}
