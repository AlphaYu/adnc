namespace Adnc.Demo.Whse.Application.Dtos;

public class ProductCreationDto : IDto
{
    public string Sku { set; get; } = string.Empty;

    public string Name { set; get; } = string.Empty;

    public string Describe { set; get; } = string.Empty;

    public decimal Price { set; get; }

    public string Unit { set; get; } = string.Empty;
}
