namespace Adnc.Demo.Maint.Repository.Entities;

/// <summary>
/// 通知
/// </summary>
public class Notice : EfFullAuditEntity
{
    public string Content { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int Type { get; set; }
}