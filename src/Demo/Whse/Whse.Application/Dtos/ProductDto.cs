namespace Adnc.Demo.Whse.Application.Dtos;

public class ProductDto : IDto
{
    public long Id { set; get; }

    public string Unit { set; get; } = string.Empty;

    public string Sku { set; get; } = string.Empty;

    public string Name { set; get; } = string.Empty;

    public string Describe { set; get; } = string.Empty;

    public decimal Price { set; get; }

    public int StatusCode { get; set; }

    public string StatusDescription { get; set; } = string.Empty;

    public string StatusChangesReason { get; set; } = string.Empty;
}
