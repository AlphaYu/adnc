namespace Adnc.Demo.Remote.Http.Messages;

public class ProductResponse()
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
