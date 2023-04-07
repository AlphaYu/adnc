namespace Adnc.Whse.Application.Dtos;

public class ProductSearchListDto : SearchDto
{
    public long[] Ids { get; set; }

    public int StatusCode { get; set; }
}