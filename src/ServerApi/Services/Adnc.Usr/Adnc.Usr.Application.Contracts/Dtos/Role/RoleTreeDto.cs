using Adnc.Infra.Application.Dtos;
using System;
using System.Collections.Generic;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    [Serializable]
    public class RoleTreeDto : IDto
    {
        public IEnumerable<Node<long>> TreeData { get; set; }
        public IEnumerable<long> CheckedIds { get; set; }
    }
}