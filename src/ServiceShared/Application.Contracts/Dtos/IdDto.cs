namespace Adnc.Shared.Application.Contracts.Dtos;

[Serializable]
public sealed class IdDto : OutputDto
{
    public IdDto()
    {
    }

    public IdDto(long id)
    {
        Id = id;
    }
}
