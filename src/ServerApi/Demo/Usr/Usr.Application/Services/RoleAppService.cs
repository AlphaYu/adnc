namespace Adnc.Demo.Usr.Application.Services;

public class RoleAppService : AbstractAppService, IRoleAppService
{
    private readonly IEfRepository<Role> _roleRepository;
    private readonly IEfRepository<User> _userRepository;
    private readonly IEfRepository<RoleRelation> _relationRepository;
    private readonly CacheService _cacheService;

    public RoleAppService(IEfRepository<Role> roleRepository,
        IEfRepository<User> userRepository,
        IEfRepository<RoleRelation> relationRepository,
        CacheService cacheService)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _relationRepository = relationRepository;
        _cacheService = cacheService;
    }

    public async Task<AppSrvResult<long>> CreateAsync(RoleCreationDto input)
    {
        input.TrimStringFields();
        var isExists = (await _cacheService.GetAllRolesFromCacheAsync()).Any(x => x.Name == input.Name);
        if (isExists)
            return Problem(HttpStatusCode.BadRequest, "该角色名称已经存在");

        var role = Mapper.Map<Role>(input);
        role.Id = IdGenerater.GetNextId();
        await _roleRepository.InsertAsync(role);

        return role.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, RoleUpdationDto input)
    {
        input.TrimStringFields();
        var isExists = (await _cacheService.GetAllRolesFromCacheAsync()).Any(x => x.Name == input.Name && x.Id != id);
        if (isExists)
            return Problem(HttpStatusCode.BadRequest, "该角色名称已经存在");

        var role = Mapper.Map<Role>(input);
        role.Id = id;
        await _roleRepository.UpdateAsync(role, UpdatingProps<Role>(x => x.Name, x => x.Tips, x => x.Ordinal));

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        if (id == 1600000000010)
            return Problem(HttpStatusCode.Forbidden, "禁止删除初始角色");

        if (await _userRepository.AnyAsync(x => x.RoleIds == id.ToString()))
            return Problem(HttpStatusCode.Forbidden, "有用户使用该角色，禁止删除");

        await _roleRepository.DeleteAsync(id);

        return AppSrvResult();
    }

    public async Task<AppSrvResult> SetPermissonsAsync(RoleSetPermissonsDto input)
    {
        if (input.RoleId == 1600000000010)
            return Problem(HttpStatusCode.Forbidden, "禁止设置初始角色");

        await _relationRepository.DeleteRangeAsync(x => x.RoleId == input.RoleId);

        var relations = new List<RoleRelation>();
        foreach (var permissionId in input.Permissions)
        {
            relations.Add(
                new RoleRelation
                {
                    Id = IdGenerater.GetNextId(),
                    RoleId = input.RoleId,
                    MenuId = permissionId
                }
            );
        }
        await _relationRepository.InsertRangeAsync(relations);

        return AppSrvResult();
    }

    public async Task<RoleTreeDto> GetRoleTreeListByUserIdAsync(long userId)
    {
        RoleTreeDto result = null;
        IEnumerable<ZTreeNodeDto<long, dynamic>> treeNodes = null;

        var user = await _userRepository.FetchAsync(x => new { x.RoleIds }, x => x.Id == userId);
        if (user is null)
            return null;

        var roles = await _cacheService.GetAllRolesFromCacheAsync();
        var roleIds = user.RoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)) ?? new List<long>();
        if (roles.Any())
        {
            treeNodes = roles.Select(x => new ZTreeNodeDto<long, dynamic>
            {
                Id = x.Id,
                PID = x.Pid ?? 0,
                Name = x.Name,
                Open = !(x.Pid.HasValue && x.Pid.Value > 0),
                Checked = roleIds.Contains(x.Id)
            });

            result = new RoleTreeDto
            {
                TreeData = treeNodes.Select(x => new Node<long>
                {
                    Id = x.Id,
                    PID = x.PID,
                    Name = x.Name,
                    Checked = x.Checked
                }),
                CheckedIds = treeNodes.Where(x => x.Checked).Select(x => x.Id)
            };
        }

        return result;
    }

    public async Task<PageModelDto<RoleDto>> GetPagedAsync(RolePagedSearchDto search)
    {
        search.TrimStringFields();
        var whereExpression = ExpressionCreator
                                              .New<Role>()
                                              .AndIf(search.RoleName.IsNotNullOrWhiteSpace(), x => x.Name.Contains(search.RoleName));

        var total = await _roleRepository.CountAsync(whereExpression);
        if (total == 0)
            return new PageModelDto<RoleDto>(search);

        var entities = await _roleRepository
                            .Where(whereExpression)
                            .OrderByDescending(x => x.Id)
                            .Skip(search.SkipRows())
                            .Take(search.PageSize)
                            .ToListAsync();
        var dtos = Mapper.Map<List<RoleDto>>(entities);

        return new PageModelDto<RoleDto>(search, dtos, total);
    }
}