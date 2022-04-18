namespace Adnc.Usr.Application.Services;

public class DeptAppService : AbstractAppService, IDeptAppService
{
    private readonly IEfRepository<SysDept> _deptRepository;
    private readonly CacheService _cacheService;

    public DeptAppService(IEfRepository<SysDept> deptRepository
        , CacheService cacheService)
    {
        _deptRepository = deptRepository;
        _cacheService = cacheService;
    }

    public async Task<AppSrvResult> DeleteAsync(long Id)
    {
        var dept = (await _cacheService.GetAllDeptsFromCacheAsync()).FirstOrDefault(x => x.Id == Id);
        var deletingPids = $"{dept.Pids}[{Id}],";
        await _deptRepository.DeleteRangeAsync(d => d.Pids.StartsWith(deletingPids) || d.Id == dept.Id);

        return AppSrvResult();
    }

    public async Task<AppSrvResult<long>> CreateAsync(DeptCreationDto input)
    {
        var isExists = (await _cacheService.GetAllDeptsFromCacheAsync()).Exists(x => x.FullName == input.FullName);
        if (isExists)
            return Problem(HttpStatusCode.BadRequest, "该部门全称已经存在");

        var dept = Mapper.Map<SysDept>(input);
        dept.Id = IdGenerater.GetNextId();
        await this.SetDeptPids(dept);
        await _deptRepository.InsertAsync(dept);

        return dept.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, DeptUpdationDto input)
    {
        var allDepts = await _cacheService.GetAllDeptsFromCacheAsync();

        var oldDeptDto = allDepts.FirstOrDefault(x => x.Id == id);
        if (oldDeptDto.Pid == 0 && input.Pid > 0)
            return Problem(HttpStatusCode.BadRequest, "一级单位不能修改等级");

        var isExists = allDepts.Exists(x => x.FullName == input.FullName && x.Id != id);
        if (isExists)
            return Problem(HttpStatusCode.BadRequest, "该部门全称已经存在");

        var deptEnity = Mapper.Map<SysDept>(input);
        deptEnity.Id = id;

        if (oldDeptDto.Pid == input.Pid)
        {
            await _deptRepository.UpdateAsync(deptEnity, UpdatingProps<SysDept>(x => x.FullName, x => x.SimpleName, x => x.Tips, x => x.Ordinal));
        }
        else
        {
            await this.SetDeptPids(deptEnity);
            await _deptRepository.UpdateAsync(deptEnity, UpdatingProps<SysDept>(x => x.FullName, x => x.SimpleName, x => x.Tips, x => x.Ordinal, x => x.Pid, x => x.Pids));

            //zz.efcore 不支持
            //await _deptRepository.UpdateRangeAsync(d => d.Pids.Contains($"[{dept.ID}]"), c => new SysDept { Pids = c.Pids.Replace(oldDeptPids, dept.Pids) });
            var originalDeptPids = $"{oldDeptDto.Pids}[{deptEnity.Id}],";
            var nowDeptPids = $"{deptEnity.Pids}[{deptEnity.Id}],";
            var subDepts = await _deptRepository
                                                                         .Where(d => d.Pids.StartsWith(originalDeptPids))
                                                                         .Select(d => new { d.Id, d.Pids })
                                                                         .ToListAsync();
            foreach (var c in subDepts)
            {
                await _deptRepository.UpdateAsync(new SysDept { Id = c.Id, Pids = c.Pids.Replace(originalDeptPids, nowDeptPids) }, UpdatingProps<SysDept>(c => c.Pids));
            }
        }

        return AppSrvResult();
    }

    public async Task<List<DeptTreeDto>> GetTreeListAsync()
    {
        var result = new List<DeptTreeDto>();
        var depts = await _cacheService.GetAllDeptsFromCacheAsync();
        if (depts.IsNullOrEmpty())
            return result;

        Func<DeptDto, DeptTreeDto> selector = x => new DeptTreeDto
        {
            Id = x.Id,
            SimpleName = x.SimpleName,
            FullName = x.FullName,
            Ordinal = x.Ordinal,
            Pid = x.Pid,
            Pids = x.Pids,
            Tips = x.Tips,
            Version = x.Version
        };

        var roots = depts.Where(d => d.Pid == 0)
                                    .OrderBy(d => d.Ordinal)
                                    .Select(selector)
                                    .ToList();
        roots.ForEach(node =>
        {
            GetChildren(node, depts);
            result.Add(node);
        });

        void GetChildren(DeptTreeDto currentNode, List<DeptDto> allDeptNodes)
        {
            var childrenNodes = depts.Where(d => d.Pid == currentNode.Id)
                                                       .OrderBy(d => d.Ordinal)
                                                       .Select(selector)
                                                       .ToList();
            if (childrenNodes.IsNotNullOrEmpty())
            {
                currentNode.Children.AddRange(childrenNodes);
                foreach (var node in childrenNodes)
                {
                    GetChildren(node, allDeptNodes);
                }
            }
        }

        return result;
    }

    private async Task<SysDept> SetDeptPids(SysDept sysDept)
    {
        if (sysDept.Pid.HasValue && sysDept.Pid.Value > 0)
        {
            var dept = (await _cacheService.GetAllDeptsFromCacheAsync()).FirstOrDefault(x => x.Id == sysDept.Pid.Value);
            string pids = dept?.Pids ?? "";
            sysDept.Pids = $"{pids}[{sysDept.Pid}],";
        }
        else
        {
            sysDept.Pid = 0;
            sysDept.Pids = "[0],";
        }
        return sysDept;
    }
}