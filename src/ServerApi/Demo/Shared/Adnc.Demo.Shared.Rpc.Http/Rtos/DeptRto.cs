namespace Adnc.Demo.Shared.Rpc.Http.Rtos;

public class DeptRto
{
    /// <summary>
    /// 构造函数
    /// 修复Warning, add by garfield 20220530
    /// </summary>
    public DeptRto()
    {
        FullName = "";
        Pids = "";
        SimpleName = "";
        Tips = "";
        Children = new List<DeptRto>();
    }

    public long Id { get; set; }

    /// <summary>
    /// 部门全称
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Ordinal { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public long? Pid { get; set; }

    /// <summary>
    /// 父级Id集合
    /// </summary>
    public string Pids { get; set; }

    /// <summary>
    /// 部门简称
    /// </summary>
    public string SimpleName { get; set; }

    /// <summary>
    /// 部门描述
    /// </summary>
    public string Tips { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 子部门
    /// </summary>
    public List<DeptRto> Children { get; set; }
}