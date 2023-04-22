namespace Adnc.Demo.Shared.Rpc.Http.Rtos;

public class ProductRto
{
    /// <summary>
    /// 构造函数
    /// 修复Warning, add by garfield 20220530
    /// </summary>
    public ProductRto(string name) => Name = name;

    public long Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}