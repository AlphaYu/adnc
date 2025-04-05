namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

public class UserCreationDto : UserCreationAndUpdationDto
{
    /// <summary>
    /// 账户
    /// </summary>
    public string Account { get; set; } = string.Empty;
}
