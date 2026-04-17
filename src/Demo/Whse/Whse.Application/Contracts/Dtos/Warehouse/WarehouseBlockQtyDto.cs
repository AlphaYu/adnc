namespace Adnc.Demo.Whse.Application.Contracts.Dtos.Warehouse;

public class WarehouseBlockQtyDto : IDto
{
    public long OrderId { get; set; }

    public IEnumerable<(long ProductId, int Qty)> Products { get; set; } = [];
}
