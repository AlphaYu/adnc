namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// System parameter
/// </summary>
public class SysConfig : EfFullAuditEntity
{
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
