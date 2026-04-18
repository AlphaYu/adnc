namespace Adnc.Shared.Application.Contracts.Dtos;

public static class SearchPagedDtoExtension
{
    /// <summary>
    /// Calculates the number of rows to skip.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static int SkipRows(this SearchPagedDto dto) => (dto.PageIndex - 1) * dto.PageSize;
}
