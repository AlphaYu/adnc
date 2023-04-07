namespace Adnc.Cust.Application.Contracts.Dtos;

public class CustomerSearchPagedDto : SearchPagedDto
{
    public long Id { get; set; }

    public string Account { get; set; }
}