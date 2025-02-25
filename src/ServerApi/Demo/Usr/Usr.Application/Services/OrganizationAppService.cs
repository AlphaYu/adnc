namespace Adnc.Demo.Usr.Application.Services;

public class OrganizationAppService : AbstractAppService, IOrganizationAppService
{
    private readonly IEfRepository<Organization> _organizationRepository;
    private readonly CacheService _cacheService;

    public OrganizationAppService(IEfRepository<Organization> organizationRepository
        , CacheService cacheService)
    {
        _organizationRepository = organizationRepository;
        _cacheService = cacheService;
    }

    public async Task<AppSrvResult> DeleteAsync(long Id)
    {
        var organization = (await _cacheService.GetAllOrganizationsFromCacheAsync()).FirstOrDefault(x => x.Id == Id);
        var deletingPids = $"{organization.Pids}[{Id}],";
        await _organizationRepository.DeleteRangeAsync(d => d.Pids.StartsWith(deletingPids) || d.Id == organization.Id);

        return AppSrvResult();
    }

    public async Task<AppSrvResult<long>> CreateAsync(OrganizationCreationDto input)
    {
        input.TrimStringFields();
        var isExists = (await _cacheService.GetAllOrganizationsFromCacheAsync()).Exists(x => x.FullName == input.FullName);
        if (isExists)
            return Problem(HttpStatusCode.BadRequest, "该机构全称已经存在");

        var organization = Mapper.Map<Organization>(input);
        organization.Id = IdGenerater.GetNextId();
        await this.SetorganizationPids(organization);
        await _organizationRepository.InsertAsync(organization);

        return organization.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, OrganizationUpdationDto input)
    {
        input.TrimStringFields();
        var allorganizations = await _cacheService.GetAllOrganizationsFromCacheAsync();

        var oldorganizationDto = allorganizations.FirstOrDefault(x => x.Id == id);
        if (oldorganizationDto.Pid == 0 && input.Pid > 0)
            return Problem(HttpStatusCode.BadRequest, "一级单位不能修改等级");

        var isExists = allorganizations.Exists(x => x.FullName == input.FullName && x.Id != id);
        if (isExists)
            return Problem(HttpStatusCode.BadRequest, "该机构全称已经存在");

        var organizationEnity = Mapper.Map<Organization>(input);
        organizationEnity.Id = id;

        if (oldorganizationDto.Pid == input.Pid)
        {
            await _organizationRepository.UpdateAsync(organizationEnity, UpdatingProps<Organization>(x => x.FullName, x => x.SimpleName, x => x.Tips, x => x.Ordinal));
        }
        else
        {
            await this.SetorganizationPids(organizationEnity);
            await _organizationRepository.UpdateAsync(organizationEnity, UpdatingProps<Organization>(x => x.FullName, x => x.SimpleName, x => x.Tips, x => x.Ordinal, x => x.Pid, x => x.Pids));

            //zz.efcore 不支持
            //await _organizationRepository.UpdateRangeAsync(d => d.Pids.Contains($"[{organization.ID}]"), c => new Sysorganization { Pids = c.Pids.Replace(oldorganizationPids, organization.Pids) });
            var originalorganizationPids = $"{oldorganizationDto.Pids}[{organizationEnity.Id}],";
            var noworganizationPids = $"{organizationEnity.Pids}[{organizationEnity.Id}],";
            var suborganizations = await _organizationRepository
                                                                         .Where(d => d.Pids.StartsWith(originalorganizationPids))
                                                                         .Select(d => new { d.Id, d.Pids })
                                                                         .ToListAsync();
            foreach (var c in suborganizations)
            {
                await _organizationRepository.UpdateAsync(new Organization { Id = c.Id, Pids = c.Pids.Replace(originalorganizationPids, noworganizationPids) }, UpdatingProps<Organization>(c => c.Pids));
            }
        }

        return AppSrvResult();
    }

    public async Task<List<OrganizationTreeDto>> GetTreeListAsync()
    {
        var result = new List<OrganizationTreeDto>();
        var organizations = await _cacheService.GetAllOrganizationsFromCacheAsync();
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

        var roots = organizations.Where(d => d.Pid == 0)
                                    .OrderBy(d => d.Ordinal)
                                    .Select(selector)
                                    .ToList();
        roots.ForEach(node =>
        {
            GetChildren(node, organizations);
            result.Add(node);
        });

        void GetChildren(OrganizationTreeDto currentNode, List<OrganizationDto> allorganizationNodes)
        {
            var childrenNodes = organizations.Where(d => d.Pid == currentNode.Id)
                                                       .OrderBy(d => d.Ordinal)
                                                       .Select(selector)
                                                       .ToList();
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

    private async Task<Organization> SetorganizationPids(Organization sysorganization)
    {
        if (sysorganization.Pid.HasValue && sysorganization.Pid.Value > 0)
        {
            var organization = (await _cacheService.GetAllOrganizationsFromCacheAsync()).FirstOrDefault(x => x.Id == sysorganization.Pid.Value);
            string pids = organization?.Pids ?? "";
            sysorganization.Pids = $"{pids}[{sysorganization.Pid}],";
        }
        else
        {
            sysorganization.Pid = 0;
            sysorganization.Pids = "[0],";
        }
        return sysorganization;
    }
}