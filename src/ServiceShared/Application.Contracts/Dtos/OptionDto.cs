namespace Adnc.Shared.Application.Contracts.Dtos;

public sealed class OptionTreeDto
{
    public string Label { get; set; } = string.Empty;

    public long Value { get; set; }

    public List<OptionTreeDto> Children { get; set; } = [];
}
