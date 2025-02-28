namespace Adnc.Demo.Cust.Api.Application.Dtos;

public class CustomerSearchPagedDto : SearchPagedDto
{
    public long? Id { get; set; }

    public string? Account { get; set; }
}