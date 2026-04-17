namespace Adnc.Demo.Whse.Application.Contracts.Dtos.Product;

public class ProductSearchListDto : SearchDto
{
    public long[] Ids { get; set; } = [];

    public int StatusCode { get; set; }
}
