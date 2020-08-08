using Adnc.Application.Dtos;
using Adnc.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    [Serializable]
    public class MenuTreeDto : BaseDto
    {
        public IEnumerable<Node<long>> TreeData { get; set; }
        public IEnumerable<long> CheckedIds { get; set; }
    }
}
