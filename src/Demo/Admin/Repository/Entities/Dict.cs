namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// 字典
/// </summary>
public class Dict : EfFullAuditEntity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Remark { get; set; } = string.Empty;

    public bool Status { get; set; }
}

/// <summary>
/// 字典数据
/// </summary>
public class DictData : EfFullAuditEntity
{
    public string DictCode { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string TagType { get; set; } = string.Empty;

    public bool Status { get; set; }

    public int Ordinal { get; set; }
}
