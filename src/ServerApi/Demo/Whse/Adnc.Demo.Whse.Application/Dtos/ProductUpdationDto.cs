namespace Adnc.Demo.Whse.Application.Dtos;

public class ProductUpdationDto : IDto
{
    public string Sku { set; get; }

    public string Name { set; get; }

    public string Describe { set; get; }

    public string Unit { set; get; }

    public decimal Price { set; get; }
}