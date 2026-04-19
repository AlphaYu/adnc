using Adnc.Demo.Admin.Application.Contracts.Dtos.Organization;

namespace Adnc.Demo.Admin.Application.Services;

/// <inheritdoc cref="IOrganizationService"/>
public class OrganizationService(IEfRepository<Organization> organizationRepo, CacheService cacheService)
    : AbstractAppService, IOrganizationService
{
    /// <inheritdoc />
    public async Task<ServiceResult<IdDto>> CreateAsync(OrganizationCreationDto input)
    {
        input.TrimStringFields();
        var exists = await organizationRepo.AnyAsync(x => x.Name == input.Name);
        if (exists)
        {
            return Problem(HttpStatusCode.BadRequest, "This organization full name already exists");
        }

        var organization = Mapper.Map<Organization>(input, IdGenerater.GetNextId());
        organization.ParentIds = await GetParentIdsAsync(organization.ParentId);
        await organizationRepo.InsertAsync(organization);

        return new IdDto(organization.Id);
    }

    /// <inheritdoc />
    public async Task<ServiceResult> UpdateAsync(long id, OrganizationUpdationDto input)
    {
        input.TrimStringFields();

        var organization = await organizationRepo.FetchAsync(x => x.Id == id, noTracking: false);
        if (organization is null)
        {
            return Problem(HttpStatusCode.NotFound, "This organization does not exist");
        }

        var exists = await organizationRepo.AnyAsync(x => x.Name == input.Name && x.Id != id);
        if (exists)
        {
            return Problem(HttpStatusCode.BadRequest, "This organization full name already exists");
        }

        if (organization.ParentId == 0 && input.ParentId > 0)
        {
            return Problem(HttpStatusCode.BadRequest, "The top-level organization cannot change its hierarchy");
        }

        var oldParentId = organization.ParentId;

        var newOrganization = Mapper.Map(input, organization);
        if (oldParentId != input.ParentId)
        {
            var oldPids = $"{organization.ParentIds}";
            var newPids = await GetParentIdsAsync(input.ParentId);
            newOrganization.ParentId = input.ParentId;
            newOrganization.ParentIds = newPids;
            var oldSubOrgPids = $"{oldPids}[{id}]";
            var newSubOrgPids = $"{newPids}[{id}]";
            await organizationRepo.ExecuteUpdateAsync(x => x.ParentIds.StartsWith(oldSubOrgPids), setters => setters.SetProperty(x => x.ParentIds, y => y.ParentIds.Replace(oldSubOrgPids, newSubOrgPids)));
        }
        await organizationRepo.UpdateAsync(newOrganization);

        return ServiceResult();
    }

    /// <inheritdoc />
    public async Task<ServiceResult> DeleteAsync(long[] ids)
    {
        foreach (var id in ids)
        {
            var organization = await organizationRepo.FetchAsync(x => new { x.Id, x.ParentIds }, x => x.Id == id);
            if (organization is not null)
            {
                var needDeletedPids = $"{organization.ParentIds}[{id}]";
                await organizationRepo.ExecuteDeleteAsync(d => d.ParentIds.StartsWith(needDeletedPids) || d.Id == organization.Id);
            }
        }
        return ServiceResult();
    }

    /// <inheritdoc />
    public async Task<OrganizationDto?> GetAsync(long id)
    {
        var org = (await cacheService.GetAllOrganizationsFromCacheAsync()).FirstOrDefault(x => x.Id == id);
        return org;
    }

    /// <inheritdoc />
    public async Task<List<OrganizationTreeDto>> GetTreeListAsync(string? name = null, bool? status = null)
    {
        var whereExpr = ExpressionCreator
            .New<OrganizationDto>()
            .AndIf(name.IsNotNullOrEmpty(), x => x.Name == name)
            .AndIf(status is not null, x => x.Status == status);
        var orgs = (await cacheService.GetAllOrganizationsFromCacheAsync()).Where(whereExpr.Compile()).OrderBy(x => x.ParentId).ThenBy(x => x.Ordinal);
        if (orgs.IsNullOrEmpty())
        {
            return [];
        }

        List<OrganizationTreeDto> GetChildren(long id)
        {
            var orgTree = new List<OrganizationTreeDto>();
            var parentOrgDtos = orgs.Where(x => x.ParentId == id);
            foreach (var orgDto in parentOrgDtos)
            {
                var orgNode = Mapper.Map<OrganizationTreeDto>(orgDto);
                orgNode.Children = GetChildren(orgDto.Id);
                orgTree.Add(orgNode);
            }
            return orgTree;
        }

        var rootId = orgs.First().ParentId;
        return GetChildren(rootId);
    }

    /// <inheritdoc />
    public async Task<List<OptionTreeDto>> GetOrgOptionsAsync(bool? status = null)
    {
        var orgTree = await GetTreeListAsync(null, status);
        return Mapper.Map<List<OptionTreeDto>>(orgTree);
    }

    /// <summary>
    /// Builds the serialized parent ID path for an organization.
    /// </summary>
    /// <param name="pid">The parent organization ID.</param>
    /// <returns>The serialized parent ID path.</returns>
    private async Task<string> GetParentIdsAsync(long pid)
    {
        var rootPids = "[0]";
        var superiorPids = await organizationRepo.FetchAsync(x => x.ParentIds, x => x.Id == pid) ?? rootPids;
        if (superiorPids == rootPids)
        {
            return rootPids;
        }
        else
        {
            return $"{superiorPids}[{pid}]";
        }
    }
}
