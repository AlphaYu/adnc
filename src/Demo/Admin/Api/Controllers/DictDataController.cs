using Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages dictionary data entries.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/dictdatas")]
[ApiController]
public class DictDataController(IDictDataService dictDataService) : AdncControllerBase
{
    /// <summary>
    /// Creates a dictionary data entry.
    /// </summary>
    /// <param name="input">The dictionary data entry to create.</param>
    /// <returns>The ID of the created dictionary data entry.</returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.DictData.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] DictDataCreationDto input)
        => CreatedResult(await dictDataService.CreateAsync(input));

    /// <summary>
    /// Updates a dictionary data entry.
    /// </summary>
    /// <param name="id">The dictionary data entry ID.</param>
    /// <param name="input">The dictionary data entry changes.</param>
    /// <returns>A result indicating whether the dictionary data entry was updated.</returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.DictData.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] DictDataUpdationDto input)
        => Result(await dictDataService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes one or more dictionary data entries.
    /// </summary>
    /// <param name="ids">The comma-separated dictionary data entry IDs.</param>
    /// <returns>A result indicating whether the dictionary data entries were deleted.</returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.DictData.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',').Select(long.Parse).ToArray();
        return Result(await dictDataService.DeleteAsync(idArr));
    }

    /// <summary>
    /// Gets a paged list of dictionary data entries.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of dictionary data entries.</returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.DictData.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<DictDataDto>>> GetPagedAsync([FromQuery] DictDataSearchPagedDto input)
        => await dictDataService.GetPagedAsync(input);

    /// <summary>
    /// Gets a dictionary data entry by ID.
    /// </summary>
    /// <param name="id">The dictionary data entry ID.</param>
    /// <returns>The requested dictionary data entry.</returns>
    [HttpGet("{id}")]
    [AdncAuthorize([PermissionConsts.DictData.Get, PermissionConsts.DictData.Update])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DictDataDto>> GetAsync([FromRoute] long id)
    {
        var dictData = await dictDataService.GetAsync(id);
        return dictData is null ? NotFound() : dictData;
    }
}
