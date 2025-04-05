namespace Adnc.Demo.Remote.Http.Messages;

public class SysConfigSimpleResponse
{
    /// <summary>
    /// 参数键
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 参数名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 参数值
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
