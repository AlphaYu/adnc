namespace Adnc.Demo.Ord.Application.Dtos;

public class OrderReceiverDto : IDto
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
