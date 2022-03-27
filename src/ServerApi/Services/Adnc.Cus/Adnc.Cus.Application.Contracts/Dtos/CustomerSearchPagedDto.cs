namespace Adnc.Cus.Application.Contracts.Dtos;

public class CustomerSearchPagedDto : SearchPagedDto
{
    public long Id { get; set; }

    public string Account { get; set; }
}