using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    [Serializable]
    public class MenuTreeDto : IDto
    {
        public IEnumerable<Node<long>> TreeData { get; set; }
        public IEnumerable<long> CheckedIds { get; set; }
    }
}