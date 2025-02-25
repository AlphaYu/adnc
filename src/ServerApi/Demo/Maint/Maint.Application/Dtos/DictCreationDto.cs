namespace Adnc.Demo.Maint.Application.Dtos;

public class DictCreationDto : InputDto
{
    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public int Ordinal { get; set; }

    public IList<DictCreationDto> Children { get; set; } = new List<DictCreationDto>();
}