namespace Adnc.Demo.Whse.Application.Dtos;

public class WarehouseDto : IDto
{
    public long Id { get; set; }

    public long? ProductId { set; get; }

    public int Qty { set; get; }

    public int FreezedQty { set; get; }

    public string PositionCode { get; set; } = string.Empty;

    public string PositionDescription { get; set; } = string.Empty;

    public string ProductSku { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;
}
