using Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages dictionaries.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/dicts")]
[ApiController]
public class DictController(IDictService dictService) : AdncControllerBase
{
    /// <summary>
    /// Creates a dictionary.
    /// </summary>
    /// <param name="input">The dictionary to create.</param>
    /// <returns>The ID of the created dictionary.</returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Dict.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] DictCreationDto input)
        => CreatedResult(await dictService.CreateAsync(input));

    /// <summary>
    /// Updates a dictionary.
    /// </summary>
    /// <param name="id">The dictionary ID.</param>
    /// <param name="input">The dictionary changes.</param>
    /// <returns>A result indicating whether the dictionary was updated.</returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Dict.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] DictUpdationDto input)
        => Result(await dictService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes one or more dictionaries.
    /// </summary>
    /// <param name="ids">The comma-separated dictionary IDs.</param>
    /// <returns>A result indicating whether the dictionaries were deleted.</returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.Dict.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',').Select(long.Parse).ToArray();
        return Result(await dictService.DeleteAsync(idArr));
    }

    /// <summary>
    /// Gets a paged list of dictionaries.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of dictionaries.</returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Dict.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<DictDto>>> GetPagedAsync([FromQuery] SearchPagedDto input)
        => await dictService.GetPagedAsync(input);

    /// <summary>
    /// Gets a dictionary by ID.
    /// </summary>
    /// <param name="id">The dictionary ID.</param>
    /// <returns>The requested dictionary.</returns>
    [HttpGet("{id}")]
    [AdncAuthorize([PermissionConsts.Dict.Get, PermissionConsts.Dict.Update])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DictDto>> GetAsync([FromRoute] long id)
    {
        var dict = await dictService.GetAsync(id);
        return dict is null ? NotFound() : dict;
    }

    /// <summary>
    /// Gets dictionary option data for the specified dictionary codes.
    /// </summary>
    /// <param name="codes">The dictionary codes.</param>
    /// <returns>The option data grouped by dictionary code.</returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DictOptionDto>>> GetOptionsAsync([FromQuery] string codes)
        => await dictService.GetOptionsAsync(codes);
}
