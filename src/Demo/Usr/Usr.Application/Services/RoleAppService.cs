namespace Adnc.Demo.Usr.Application.Services;

public class RoleAppService(IEfRepository<Role> roleRepo, IEfRepository<RoleUserRelation> roleUserRelationRepo, IEfRepository<RoleMenuRelation> roleMenuRelationRepo, CacheService cacheService)
    : AbstractAppService, IRoleAppService
{
    public async Task<AppSrvResult<long>> CreateAsync(RoleCreationDto input)
    {
        input.TrimStringFields();
        var existsCode = await roleRepo.AnyAsync(x => x.Code == input.Code);
        if (existsCode)
            return Problem(HttpStatusCode.BadRequest, "该角色代码已经存在");

        var existsName = await roleRepo.AnyAsync(x => x.Name == input.Name);
        if (existsName)
            return Problem(HttpStatusCode.BadRequest, "该角色名称已经存在");

        var role = Mapper.Map<Role>(input, IdGenerater.GetNextId());
        await roleRepo.InsertAsync(role);

        return role.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, RoleUpdationDto input)
    {
        input.TrimStringFields();

        var role = await roleRepo.FetchAsync(x => x.Id == id);
        if (role is null)
            return Problem(HttpStatusCode.BadRequest, "该角色Id不存在");

        var existsCode = await roleRepo.AnyAsync(x => x.Code == input.Code && x.Id != id);
        if (existsCode)
            return Problem(HttpStatusCode.BadRequest, "该角色代码已经存在");

        var existsName = await roleRepo.AnyAsync(x => x.Code == input.Code && x.Id != id);
        if (existsName)
            return Problem(HttpStatusCode.BadRequest, "该角色名称已经存在");

        Mapper.Map(input, role);
        await roleRepo.UpdateAsync(role);
        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        if (id == 1600000000010)
            return Problem(HttpStatusCode.Forbidden, "禁止删除初始角色");

        await roleRepo.ExecuteDeleteAsync(x => x.Id == id);
        await roleUserRelationRepo.ExecuteDeleteAsync(x => x.RoleId == id);

        return AppSrvResult();
    }

    public async Task<AppSrvResult> SetPermissonsAsync(RoleSetPermissonsDto input)
    {
        if (input.RoleId == 1600000000010)
            return Problem(HttpStatusCode.Forbidden, "禁止设置初始角色");

        await roleMenuRelationRepo.ExecuteDeleteAsync(x => x.RoleId == input.RoleId);

        if (input.Permissions.IsNotNullOrEmpty())
        {
            var relations = input.Permissions.Select(x => new RoleMenuRelation { Id = IdGenerater.GetNextId(), RoleId = input.RoleId, MenuId = x });
            await roleMenuRelationRepo.InsertRangeAsync(relations);
        }

        return AppSrvResult();
    }

    public async Task<RoleTreeDto> GetRoleTreeListByUserIdAsync(long userId)
    {
        var roleIds = await roleUserRelationRepo.Where(x => x.UserId == userId).Select(y => y.RoleId).ToListAsync();
        if (roleIds.IsNullOrEmpty())
            return new RoleTreeDto { TreeData = [], CheckedIds = [] };

        var rolesCache = await cacheService.GetAllRolesFromCacheAsync();
        if (rolesCache.IsNullOrEmpty())
            return new RoleTreeDto { TreeData = [], CheckedIds = [] };

        IEnumerable<ZTreeNodeDto<long, dynamic>> treeNodes = rolesCache.Select(x => new ZTreeNodeDto<long, dynamic>
        {
            Id = x.Id,
            PID = x.Pid,
            Name = x.Name,
            Open = !(x.Pid > 0),
            Checked = roleIds.Contains(x.Id)
        });

        var result = new RoleTreeDto
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

        return result;
    }

    public async Task<PageModelDto<RoleDto>> GetPagedAsync(RolePagedSearchDto search)
    {
        search.TrimStringFields();
        var whereExpression = ExpressionCreator
                                              .New<Role>()
                                              .AndIf(search.RoleName.IsNotNullOrWhiteSpace(), x => x.Name.Contains(search.RoleName));

        var total = await roleRepo.CountAsync(whereExpression);
        if (total == 0)
            return new PageModelDto<RoleDto>(search);

        var entities = await roleRepo
                            .Where(whereExpression)
                            .OrderByDescending(x => x.Id)
                            .Skip(search.SkipRows())
                            .Take(search.PageSize)
                            .ToListAsync();
        var dtos = Mapper.Map<List<RoleDto>>(entities);

        return new PageModelDto<RoleDto>(search, dtos, total);
    }
}