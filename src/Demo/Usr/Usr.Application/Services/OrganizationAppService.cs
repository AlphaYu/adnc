namespace Adnc.Demo.Usr.Application.Services;

public class OrganizationAppService(IEfRepository<Organization> organizationRepo, CacheService cacheService) : AbstractAppService, IOrganizationAppService
{
    public async Task<AppSrvResult<long>> CreateAsync(OrganizationCreationDto input)
    {
        input.TrimStringFields();
        var exists = await organizationRepo.AnyAsync(x => x.FullName == input.FullName);
        if (exists)
            return Problem(HttpStatusCode.BadRequest, "该机构全称已经存在");

        var organization = Mapper.Map<Organization>(input, IdGenerater.GetNextId());
        organization.Pids = await GetPids(organization.Pid);
        await organizationRepo.InsertAsync(organization);

        return organization.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, OrganizationUpdationDto input)
    {
        input.TrimStringFields();

        var organization = await organizationRepo.FetchAsync(x => x.Id == id, noTracking: false);
        if (organization is null)
            return Problem(HttpStatusCode.NotFound, "该机构不存在");

        var exists = await organizationRepo.AnyAsync(x => x.FullName == input.FullName && x.Id != id);
        if (exists)
            return Problem(HttpStatusCode.BadRequest, "该机构全称已经存在");

        if (organization.Pid == 0 && input.Pid > 0)
            return Problem(HttpStatusCode.BadRequest, "一级单位不能修改等级");

        organization.FullName = input.FullName;
        organization.SimpleName = input.SimpleName;
        organization.Ordinal = input.Ordinal;
        if (organization.Pid != input.Pid)
        {
            var oldPids = $"{organization.Pids}";
            var newPids = await GetPids(input.Pid);
            organization.Pid = input.Pid;
            organization.Pids = newPids;
            var oldSubOrgPids = $"{oldPids}[{organization.Id}]";
            var newSubOrgPids = $"{newPids}[{organization.Id}]";
            await organizationRepo.ExecuteUpdateAsync(x => x.Pids.StartsWith(oldSubOrgPids), setters => setters.SetProperty(x => x.Pids, y => y.Pids.Replace(oldSubOrgPids, newSubOrgPids)));
        }
        await organizationRepo.UpdateAsync(organization);

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long Id)
    {
        var organization = await organizationRepo.FetchAsync(x => new { x.Id, x.Pids }, x => x.Id == Id);
        if (organization is null)
            return AppSrvResult();

        var needDeletedPids = $"{organization.Pids}[{Id}]";
        await organizationRepo.ExecuteDeleteAsync(d => d.Pids.StartsWith(needDeletedPids) || d.Id == organization.Id);

        return AppSrvResult();
    }

    public async Task<List<OrganizationTreeDto>> GetTreeListAsync()
    {
        var result = new List<OrganizationTreeDto>();
        var organizations = await cacheService.GetAllOrganizationsFromCacheAsync();
        if (organizations.IsNullOrEmpty())
            return result;

        Func<OrganizationDto, OrganizationTreeDto> selector = x => new OrganizationTreeDto
        {
            Id = x.Id,
            SimpleName = x.SimpleName,
            FullName = x.FullName,
            Ordinal = x.Ordinal,
            Pid = x.Pid,
            Pids = x.Pids,
            Tips = x.Tips
        };

        var roots = organizations.Where(d => d.Pid == 0).OrderBy(d => d.Ordinal).Select(selector).ToList();
        roots.ForEach(node =>
        {
            GetChildren(node, organizations);
            result.Add(node);
        });

        void GetChildren(OrganizationTreeDto currentNode, List<OrganizationDto> allorganizationNodes)
        {
            var childrenNodes = organizations.Where(d => d.Pid == currentNode.Id) .OrderBy(d => d.Ordinal).Select(selector).ToList();
            if (childrenNodes.IsNotNullOrEmpty())
            {
                currentNode.Children.AddRange(childrenNodes);
                foreach (var node in childrenNodes)
                {
                    GetChildren(node, allorganizationNodes);
                }
            }
        }

        return result;
    }

    private async Task<string> GetPids(long pid)
    {
        var rootPids = "[0]";
        var superiorPids = await organizationRepo.FetchAsync(x => x.Pids, x => x.Id == pid) ?? rootPids;
        if (superiorPids == rootPids)
            return rootPids;
        else
            return $"{superiorPids}[{pid}]";
    }
}