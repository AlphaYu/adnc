namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 字典检索条件
/// </summary>
public class DictSearchPagedDto : SearchPagedDto
{
    public string? Keywords { get; set; }
}

/// <summary>
/// 字典数据检索条件
/// </summary>
public class DictDataSearchPagedDto : SearchPagedDto
{
    public string? Keywords { get; set; }

    public string? DictCode { get; set; }
}