namespace Adnc.Demo.Whse.Application.Dtos;

public class WarehouseCreationDto : IDto
{
    public string PositionCode { get; set; } = string.Empty;

    public string PositionDescription { get; set; } = string.Empty;
}
