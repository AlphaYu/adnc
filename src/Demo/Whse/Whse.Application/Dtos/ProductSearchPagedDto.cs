namespace Adnc.Demo.Whse.Application.Dtos;

public class ProductSearchPagedDto : SearchPagedDto
{
    public long Id { get; set; }

    public int StatusCode { get; set; }
}