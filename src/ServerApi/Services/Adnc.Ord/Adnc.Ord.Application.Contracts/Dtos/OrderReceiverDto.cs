namespace Adnc.Ord.Application.Contracts.Dtos;

public class OrderReceiverDto : IDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}