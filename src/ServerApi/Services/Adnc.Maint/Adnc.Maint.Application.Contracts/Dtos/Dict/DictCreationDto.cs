namespace Adnc.Maint.Application.Contracts.Dtos;

public class DictCreationDto : IInputDto
{
    public string Name { get; set; }

    public string Value { get; set; }

    public int Ordinal { get; set; }

    public IList<DictCreationDto> Children { get; set; }
}