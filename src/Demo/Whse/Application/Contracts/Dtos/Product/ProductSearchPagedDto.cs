namespace Adnc.Demo.Whse.Application.Contracts.Dtos.Product;

public class ProductSearchPagedDto : SearchPagedDto
{
    public long Id { get; set; }

    public int StatusCode { get; set; }
}
