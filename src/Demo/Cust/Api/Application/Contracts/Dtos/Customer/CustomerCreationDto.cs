namespace Adnc.Demo.Cust.Api.Application.Contracts.Dtos.Customer;

public class CustomerCreationDto : InputDto
{
    public string Account { get; set; } = string.Empty;

    public string Nickname { get; set; } = string.Empty;

    public string Realname { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
