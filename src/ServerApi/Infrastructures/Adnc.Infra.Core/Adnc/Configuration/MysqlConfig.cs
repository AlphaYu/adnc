namespace Adnc.Infra.Core.Configuration;

/// <summary>
/// MysqlConfig配置
/// </summary>
public class MysqlConfig
{
    public const string Name = "Mysql";

    public string ConnectionString { get; set; } = string.Empty;
}