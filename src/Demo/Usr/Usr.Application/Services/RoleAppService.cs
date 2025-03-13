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

        var role = await roleRepo.FetchAsync(x => x.Id == id, noTracking: false);
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

    public async Task<AppSrvResult> DeleteAsync(long[] ids)
    {
        if (ids.Contains(1600000000010))
            return Problem(HttpStatusCode.Forbidden, "禁止删除初始角色");

        await roleRepo.ExecuteDeleteAsync(x => ids.Contains(x.Id));
        await roleUserRelationRepo.ExecuteDeleteAsync(x => ids.Contains(x.RoleId));

        return AppSrvResult();
    }

    public async Task<RoleDto?> GetAsync(long id)
    {
        var role = await roleRepo.FetchAsync(x => x.Id == id);
        return role is null ? null : Mapper.Map<RoleDto>(role);
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

    public async Task<string[]> GetPermissionsAsync(long id)
    {
        var menuIds = (await cacheService.GetAllRoleMenusFromCacheAsync()).Where(x => x.RoleId == id).Select(x => x.MenuId.ToString()).ToArray();
        return menuIds ?? [];
    }

    public async Task<List<OptionTreeDto>> GetOptionsAsync(bool? status = null)
    {
        var whereExpr = ExpressionCreator
                                      .New<Role>()
                                      .AndIf(status is not null, x => x.Status);
        var options = await roleRepo.Where(whereExpr).Select(x => new OptionTreeDto { Label = x.Name, Value = x.Id }).ToListAsync();

        return options ?? [];
    }

    public async Task<PageModelDto<RoleDto>> GetPagedAsync(RolePagedSearchDto search)
    {
        search.TrimStringFields();
        var whereExpression = ExpressionCreator
                                              .New<Role>()
                                              .AndIf(search.Keywords.IsNotNullOrWhiteSpace(), x => x.Name.Contains(search.Keywords));

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