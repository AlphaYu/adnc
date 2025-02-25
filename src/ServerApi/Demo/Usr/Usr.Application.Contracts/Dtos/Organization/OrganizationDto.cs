namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 部门
/// </summary>
[Serializable]
public class OrganizationDto : OutputDto
{
    /// <summary>
    /// 部门全称
    /// </summary>
    public string FullName { get; set; } = default!;

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
    public string Pids { get; set; } = default!;

    /// <summary>
    /// 部门简称
    /// </summary>
    public string SimpleName { get; set; } = default!;

    /// <summary>
    /// 部门描述
    /// </summary>
    private string _tips = string.Empty;

    public string Tips
    {
        get => _tips is null ? string.Empty : _tips;
        set => _tips = value;
    }
}