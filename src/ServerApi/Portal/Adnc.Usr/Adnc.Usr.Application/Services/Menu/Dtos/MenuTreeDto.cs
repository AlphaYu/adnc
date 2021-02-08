using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Application.Services;
using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    [Serializable]
    public class MenuTreeDto : IDto
    {
        public IEnumerable<Node<long>> TreeData { get; set; }
        public IEnumerable<long> CheckedIds { get; set; }
    }
}
