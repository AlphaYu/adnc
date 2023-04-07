namespace Adnc.Shared.Application.Contracts.Dtos;

public static class SearchPagedDtoExtension
{
    /// <summary>
    /// 计算查询需要跳过的行数
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static int SkipRows(this SearchPagedDto dto) => (dto.PageIndex - 1) * dto.PageSize;
}