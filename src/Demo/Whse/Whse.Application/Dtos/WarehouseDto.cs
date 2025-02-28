namespace Adnc.Demo.Whse.Application.Dtos;

public class WarehouseDto : IDto
{
    public long Id { get; set; }

    public long? ProductId { set; get; }

    public int Qty { set; get; }

    public int FreezedQty { set; get; }

    public string PositionCode { get; set; }

    public string PositionDescription { get; set; }

    public string ProductSku { get; set; }

    public string ProductName { get; set; }
}