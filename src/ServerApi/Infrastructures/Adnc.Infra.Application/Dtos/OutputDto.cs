using System;

namespace Adnc.Infra.Application.Dtos
{
    [Serializable]
    public abstract class OutputDto : IOutputDto
    {
        public virtual long Id { get; set; }
    }
}