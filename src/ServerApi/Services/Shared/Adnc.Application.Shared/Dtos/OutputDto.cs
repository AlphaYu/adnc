using ProtoBuf;
using System;

namespace Adnc.Application.Shared.Dtos
{
    [Serializable]
    public abstract class OutputDto : IOutputDto
    {
        public virtual long Id { get; set; }
    }
}