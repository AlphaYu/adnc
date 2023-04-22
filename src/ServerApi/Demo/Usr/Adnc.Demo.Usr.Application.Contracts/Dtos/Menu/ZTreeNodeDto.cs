namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// ZTree节点
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TData"></typeparam>
public class ZTreeNodeDto<TKey, TData> : IDto
{
    /// <summary>
    /// 节点Id
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    /// 父节点Id
    /// </summary>
    public TKey PID { get; set; }

    /// <summary>
    /// 节点名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 是否展开
    /// </summary>
    public bool Open { get; set; }

    /// <summary>
    /// 是否选中
    /// </summary>
    public bool Checked { get; set; }

    /// <summary>
    /// 节点数据
    /// </summary>
    public TData Data { get; set; }

    public static ZTreeNodeDto<TKey, TData> CreateParent()
    {
        ZTreeNodeDto<TKey, TData> node = new ZTreeNodeDto<TKey, TData>
        {
            Checked = true,
            Id = default,
            Name = "顶级",
            Open = true,
            PID = default
        };

        return node;
    }
}