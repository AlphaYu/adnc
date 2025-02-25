namespace Adnc.Demo.Shared.Rpc.Http.Rtos;

public class DictRto
{
    /// <summary>
    /// 构造函数
    /// 修复Warning, add by garfield 20220530
    /// </summary>
    public DictRto()
    {
        Name = "";
        Num = "";
        Value = "";
    }

    public long ID { get; set; }

    public string Name { get; set; }

    public string Num { get; set; }

    public long? Pid { get; set; }

    public string Value { get; set; }

    private IReadOnlyList<DictRto> _data = Array.Empty<DictRto>();

    public IReadOnlyList<DictRto> Children
    {
        get => _data;
        set
        {
            if (value != null)
            {
                _data = value;
            }
        }
    }
}