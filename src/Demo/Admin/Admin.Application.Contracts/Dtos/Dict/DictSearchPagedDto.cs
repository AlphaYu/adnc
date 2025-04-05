namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 字典数据检索条件
/// </summary>
public class DictDataSearchPagedDto : SearchPagedDto
{
    public string? DictCode { get; set; }
}
