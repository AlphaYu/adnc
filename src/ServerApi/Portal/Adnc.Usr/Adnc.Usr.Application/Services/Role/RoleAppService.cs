using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Usr.Application.Dtos;
using Adnc.Infr.Common.Extensions;
using Adnc.Usr.Core.CoreServices;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Microsoft.EntityFrameworkCore;
using EasyCaching.Core;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared;

namespace Adnc.Usr.Application.Services
{
    public class RoleAppService :AppService,IRoleAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysRole> _roleRepository;
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IEfRepository<SysRelation> _relationRepository;
        private readonly IUsrManagerService _systemManagerService;
        private readonly IHybridCachingProvider _cache;

        public RoleAppService(IMapper mapper,
            IEfRepository<SysRole> roleRepository,
            IEfRepository<SysUser> userRepository,
            IEfRepository<SysRelation> relationRepository,
            IUsrManagerService systemManagerService,
            IHybridProviderFactory hybridProviderFactory)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _systemManagerService = systemManagerService;
            _relationRepository = relationRepository;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
        }

        public async Task<AppSrvResult<PageModelDto<RoleDto>>> GetPaged(RoleSearchDto searchModel)
        {
            Expression<Func<SysRole, bool>> whereCondition = x => true;
            if (searchModel.RoleName.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(searchModel.RoleName));
            }

            var pagedModel = await _roleRepository.PagedAsync(searchModel.PageIndex, searchModel.PageSize, whereCondition, x => x.ID, true);

            return _mapper.Map<PageModelDto<RoleDto>>(pagedModel);
        }

        public async Task<AppSrvResult<dynamic>> GetRoleTreeListByUserId(long userId)
        {
            dynamic result = null;
            IEnumerable<ZTreeNodeDto<long, dynamic>> treeNodes = null;
            var user = await _userRepository.FetchAsync(u => new { u.ID, u.RoleId }, x => x.ID == userId);

            if (user == null)
                return null;

            var roles = await _roleRepository.SelectAsync(r => r, x => true);
            var roleIds = user.RoleId?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)) ?? new List<long>();
            if (roles.Any())
            {
                treeNodes = roles.Select(x => new ZTreeNodeDto<long, dynamic>
                {
                    ID = x.ID,
                    PID = x.PID.HasValue ? x.PID.Value : 0,
                    Name = x.Name,
                    Open = x.PID.HasValue && x.PID.Value > 0 ? false : true,
                    Checked = roleIds.Contains(x.ID)
                });

                result = new
                {
                    treeData = treeNodes.Select(x => new Node<long>
                    {
                        ID = x.ID,
                        PID = x.PID,
                        Name = x.Name,
                        Checked = x.Checked
                    }),
                    checkedIds = treeNodes.Where(x => x.Checked).Select(x => x.ID)
                };
            }

            return result;
        }

        public async Task<AppSrvResult> Delete(long Id)
        {
            if (Id == 1600000000010)
                return Problem(HttpStatusCode.Forbidden, "禁止删除初始角色");

            if (await _userRepository.ExistAsync(x => x.RoleId == Id.ToString()))
                return Problem(HttpStatusCode.Forbidden, "有用户使用该角色，禁止删除");

            await _roleRepository.DeleteAsync(Id);

            return DefaultResult();
        }

        public async Task<AppSrvResult> SaveRolePermisson(PermissonSaveInputDto inputDto)
        {
            if (inputDto.RoleId == 1600000000010)
                return Problem(HttpStatusCode.Forbidden, "禁止设置初始角色");

            await _systemManagerService.SaveRolePermisson(inputDto.RoleId, inputDto.Permissions);

            return DefaultResult();
        }

        public async Task<AppSrvResult<long>> Add(RoleSaveInputDto roleDto)
        {
            var isExists = (await this.GetAllFromCache()).Where(x => x.Name == roleDto.Name).Any();
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该角色名称已经存在");

            var role = _mapper.Map<SysRole>(roleDto);
            role.ID = IdGenerater.GetNextId();
            await _roleRepository.InsertAsync(role);

            return role.ID;
        }

        public async Task<AppSrvResult> Update(RoleSaveInputDto roleDto)
        {
            var isExists = (await this.GetAllFromCache()).Where(x => x.Name == roleDto.Name && x.ID != roleDto.ID).Any();
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该角色名称已经存在");

            var role = _mapper.Map<SysRole>(roleDto);
            await _roleRepository.UpdateAsync(role);

            return DefaultResult();
        }

        public async ValueTask<AppSrvResult<bool>> ExistPermissions(RolePermissionsCheckInputDto inputDto)
        {
            var codes = await this.GetPermissions(inputDto);

            if (codes.IsSuccess && codes.Content.Any())
                return true;

            return false;
        }

        public async Task<AppSrvResult<List<string>>> GetPermissions(RolePermissionsCheckInputDto inputDto)
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuCodesCacheKey, async () =>
            {
                var allMenus = await _relationRepository.GetAll()
               .Where(x => x.Menu.Status == true)
               .Select(x => new RoleMenuCodesDto { RoleId = x.RoleId, Code = x.Menu.Code })
               .ToArrayAsync();
                return allMenus.Distinct().ToArray();
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            var codes = cahceValue.Value?.Where(x => inputDto.RoleIds.Contains(x.RoleId)).Select(x => x.Code.ToUpper());
            if (codes != null && codes.Any())
            {
                var result = codes.Intersect(inputDto.Permissions.Select(x => x.ToUpper()));
                return result.ToList();
            } 

            return null;
        }

        public async Task<List<RoleDto>> GetAllFromCache()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.RoleAllCacheKey, async () =>
            {
                var allRoles = await _roleRepository.GetAll().ToListAsync();
                return _mapper.Map<List<RoleDto>>(allRoles);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }
    }
}
