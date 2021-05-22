using System;

namespace Adnc.Application.Shared.Dtos
{
    [Serializable]
    public abstract class OutputDto<TKey> : IOutputDto<TKey>
    {
        public TKey Id { get; set; }
    }

    public abstract class OutputDto : OutputDto<long>
    {
    }
}