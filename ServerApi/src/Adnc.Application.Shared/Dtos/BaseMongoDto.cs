using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Shared.Dtos
{
    [Serializable]
    public abstract class BaseMongoDto : BaseDto
    {
        public string Id { get; set; }
    }
}
