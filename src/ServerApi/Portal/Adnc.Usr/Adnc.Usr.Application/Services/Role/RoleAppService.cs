using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Usr.Application.Dtos;
using Adnc.Infr.Common.Extensions;
using Adnc.Usr.Core.Entities;
using Adnc.Usr.Core.Services;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using EasyCaching.Core;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;

namespace Adnc.Usr.Application.Services
{
    public class RoleAppService :AppService,IRoleAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysRole> _roleRepository;
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IEfRepository<SysRelation> _relationRepository;
        private readonly UsrManager _usrManager;
        private readonly IHybridCachingProvider _cache;

        public RoleAppService(IMapper mapper,
            IEfRepository<SysRole> roleRepository,
            IEfRepository<SysUser> userRepository,
            IEfRepository<SysRelation> relationRepository,
            UsrManager usrManager,
            IHybridProviderFactory hybridProviderFactory)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _relationRepository = relationRepository;
            _usrManager = usrManager;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
        }

        public async Task<AppSrvResult<PageModelDto<RoleDto>>> GetPagedAsync(RolePagedSearchDto search)
        {
            Expression<Func<SysRole, bool>> whereCondition = x => true;
            if (search.RoleName.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(search.RoleName));
            }

            var pagedModel = await _roleRepository.PagedAsync(search.PageIndex, search.PageSize, whereCondition, x => x.Ordinal, true);

            return _mapper.Map<PageModelDto<RoleDto>>(pagedModel);
        }

        public async Task<AppSrvResult<dynamic>> GetRoleTreeListByUserIdAsync(long userId)
        {
            dynamic result = null;
            IEnumerable<ZTreeNodeDto<long, dynamic>> treeNodes = null;
            var user = await _userRepository.FetchAsync(u => new { u.Id, u.RoleIds }, x => x.Id == userId);

            if (user == null)
                return null;

            //var roles = await _roleRepository.SelectAsync(r => r, x => true);
            var roles = await _roleRepository.Where(x => true).ToListAsync();
            var roleIds = user.RoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)) ?? new List<long>();
            if (roles.Any())
            {
                treeNodes = roles.Select(x => new ZTreeNodeDto<long, dynamic>
                {
                    Id = x.Id,
                    PID = x.PID.HasValue ? x.PID.Value : 0,
                    Name = x.Name,
                    Open = x.PID.HasValue && x.PID.Value > 0 ? false : true,
                    Checked = roleIds.Contains(x.Id)
                });

                result = new
                {
                    treeData = treeNodes.Select(x => new Node<long>
                    {
                        Id = x.Id,
                        PID = x.PID,
                        Name = x.Name,
                        Checked = x.Checked
                    }),
                    checkedIds = treeNodes.Where(x => x.Checked).Select(x => x.Id)
                };
            }

            return result;
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

            await _usrManager.SetRolePermissonAsync(input.RoleId, input.Permissions);

            return AppSrvResult();
        }

        public async Task<AppSrvResult<long>> CreateAsync(RoleCreationDto input)
        {
            var isExists = (await this.GetAllFromCacheAsync()).Where(x => x.Name == input.Name).Any();
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该角色名称已经存在");

            var role = _mapper.Map<SysRole>(input);
            role.Id = IdGenerater.GetNextId();
            await _roleRepository.InsertAsync(role);

            return role.Id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id,RoleUpdationDto input)
        {
            var isExists = (await this.GetAllFromCacheAsync()).Where(x => x.Name == input.Name && x.Id != id).Any();
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该角色名称已经存在");

            var role = _mapper.Map<SysRole>(input);

            role.Id = id;
            
            await _roleRepository.UpdateAsync(role, UpdatingProps<SysRole>(x => x.Name, x => x.Tips, x => x.Ordinal));

            return AppSrvResult();
        }

        public async ValueTask<AppSrvResult<bool>> ExistPermissionsAsync(RolePermissionsCheckerDto input)
        {
            var codes = await this.GetPermissionsAsync(input);

            if (codes.IsSuccess && codes.Content.Any())
                return true;

            return false;
        }

        public async Task<AppSrvResult<List<string>>> GetPermissionsAsync(RolePermissionsCheckerDto input)
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuCodesCacheKey, async () =>
            {
                var allMenus = await _relationRepository.GetAll(writeDb:true)
               .Where(x => x.Menu.Status == true)
               .Select(x => new RoleMenuCodesDto { RoleId = x.RoleId, Code = x.Menu.Code })
               .ToListAsync();
                return allMenus.Distinct().ToArray();
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            var codes = cahceValue.Value?.Where(x => input.RoleIds.Contains(x.RoleId)).Select(x => x.Code.ToUpper());
            if (codes != null && codes.Any())
            {
                var result = codes.Intersect(input.Permissions.Select(x => x.ToUpper()));
                return result.ToList();
            } 

            return null;
        }

        public async Task<List<RoleDto>> GetAllFromCacheAsync()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.RoleAllCacheKey, async () =>
            {
                var allRoles = await _roleRepository.GetAll(writeDb:true).OrderBy(x=>x.Ordinal).ToListAsync();
                return _mapper.Map<List<RoleDto>>(allRoles);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }
    }
}
