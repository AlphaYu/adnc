namespace Adnc.Demo.Whse.Application.Dtos;

public class ProductDto : IDto
{
    public long Id { set; get; }

    public string Unit { set; get; }

    public string Sku { set; get; }

    public string Name { set; get; }

    public string Describe { set; get; }

    public decimal Price { set; get; }

    public int StatusCode { get; set; }

    public string StatusDescription { get; set; }

    public string StatusChangesReason { get; set; }
}