namespace Adnc.Shared.Application.Contracts.Dtos;

[Serializable]
public abstract class OutputDto : IOutputDto
{
    public virtual long Id { get; set; }
}