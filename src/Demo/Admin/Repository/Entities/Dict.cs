namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// Dictionary
/// </summary>
public class Dict : EfFullAuditEntity
{
    public static readonly int Code_MaxLength = 32;
    public static readonly int Name_MaxLength = 32;
    public static readonly int Remark_MaxLength = 128;

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Remark { get; set; } = string.Empty;

    public bool Status { get; set; }
}

/// <summary>
/// Dictionary data
/// </summary>
public class DictData : EfFullAuditEntity
{
    public const int Label_MaxLength = 32;
    public const int Value_MaxLength = 32;
    public const int TagType_MaxLength = 32;

    public string DictCode { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string TagType { get; set; } = string.Empty;

    public bool Status { get; set; }

    public int Ordinal { get; set; }
}
