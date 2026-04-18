using Adnc.Demo.Admin.Application.Contracts.Dtos.Organization;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages organizations.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/organizations")]
[ApiController]
public class OrganizationController(IOrganizationService organizationService) : AdncControllerBase
{
    /// <summary>
    /// Creates an organization.
    /// </summary>
    /// <param name="input">The organization to create.</param>
    /// <returns>The ID of the created organization.</returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Org.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] OrganizationCreationDto input)
        => CreatedResult(await organizationService.CreateAsync(input));

    /// <summary>
    /// Updates an organization.
    /// </summary>
    /// <param name="id">The organization ID.</param>
    /// <param name="input">The organization changes.</param>
    /// <returns>A result indicating whether the organization was updated.</returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Org.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] OrganizationUpdationDto input)
        => Result(await organizationService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes one or more organizations.
    /// </summary>
    /// <param name="ids">The comma-separated organization IDs.</param>
    /// <returns>A result indicating whether the organizations were deleted.</returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.Org.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] string ids)
    {
        var idArr = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        return Result(await organizationService.DeleteAsync(idArr));
    }

    /// <summary>
    /// Gets an organization by ID.
    /// </summary>
    /// <param name="id">The organization ID.</param>
    /// <returns>The requested organization.</returns>
    [HttpGet("{id}")]
    [AdncAuthorize([PermissionConsts.Org.Get, PermissionConsts.Org.Update])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrganizationDto>> GetAsync([FromRoute] long id)
    {
        var org = await organizationService.GetAsync(id);
        return org is null ? NotFound() : org;
    }

    /// <summary>
    /// Gets the organization tree.
    /// </summary>
    /// <param name="keywords">The optional organization name filter.</param>
    /// <param name="status">The optional organization status filter.</param>
    /// <returns>The organization tree.</returns>
    [HttpGet()]
    [AdncAuthorize(PermissionConsts.Org.Search, AdncAuthorizeAttribute.JwtWithBasicSchemes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrganizationTreeDto>>> GetTreeListAsync(string? keywords = null, bool? status = null)
        => await organizationService.GetTreeListAsync(keywords, status);

    /// <summary>
    /// Gets organization options.
    /// </summary>
    /// <returns>The organization option tree.</returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OptionTreeDto>>> GetOrgOptionsAsync()
        => await organizationService.GetOrgOptionsAsync(true);
}
