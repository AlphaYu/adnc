namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// System parameter
/// </summary>
public class SysConfig : EfFullAuditEntity
{
    public static readonly int Key_MaxLength = 64;
    public static readonly int Name_MaxLength = 64;
    public static readonly int Value_MaxLength = 128;
    public static readonly int Remark_MaxLength = 128;

    /// <summary>
    /// Parameter key
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Parameter name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Parameter value
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Remark
    /// </summary>
    public string Remark { get; set; } = string.Empty;
}
