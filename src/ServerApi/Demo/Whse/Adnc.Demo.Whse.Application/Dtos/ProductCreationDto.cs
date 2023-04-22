namespace Adnc.Demo.Whse.Application.Dtos;

public class ProductCreationDto : IDto
{
    public string Sku { set; get; }

    public string Name { set; get; }

    public string Describe { set; get; }

    public decimal Price { set; get; }

    public string Unit { set; get; }
}