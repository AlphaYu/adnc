namespace Adnc.Cus.Application.Contracts.Dtos;

public class CustomerRegisterDto : IInputDto
{
    public string Account { get; set; }

    public string Password { get; set; }

    public string Nickname { get; set; }

    public string Realname { get; set; }
}