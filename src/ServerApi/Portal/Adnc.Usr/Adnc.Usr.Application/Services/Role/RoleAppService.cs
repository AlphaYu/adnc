using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Infr.Common.Extensions;
using Adnc.Usr.Core.CoreServices;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Microsoft.EntityFrameworkCore;
using EasyCaching.Core;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared;

namespace Adnc.Usr.Application.Services
{
    public class RoleAppService : IRoleAppService
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

        public async Task<PageModelDto<RoleDto>> GetPaged(RoleSearchDto searchModel)
        {
            Expression<Func<SysRole, bool>> whereCondition = x => true;
            if (!string.IsNullOrWhiteSpace(searchModel.RoleName))
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(searchModel.RoleName));
            }

            var pagedModel = await _roleRepository.PagedAsync(searchModel.PageIndex, searchModel.PageSize, whereCondition, x => x.ID, true);

            return _mapper.Map<PageModelDto<RoleDto>>(pagedModel);
        }

        public async Task<dynamic> GetRoleTreeListByUserId(long userId)
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

        public async Task Delete(long Id)
        {
            if (Id < 2)
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden, "不能删除初始角色"));
            }

            if (await _userRepository.ExistAsync(x => x.RoleId == Id.ToString()))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden, "有用户使用该角色，禁止删除"));
            }

            await _roleRepository.DeleteAsync(new long[] { Id });
        }

        public async Task SaveRolePermisson(PermissonSaveInputDto inputDto)
        {
            await _systemManagerService.SaveRolePermisson(inputDto.RoleId, inputDto.Permissions);
        }

        public async Task Save(RoleSaveInputDto roleDto)
        {
            var role = _mapper.Map<SysRole>(roleDto);
            if (roleDto.ID < 1)
            {
                role.ID = IdGenerater.GetNextId();
                await _roleRepository.InsertAsync(role);
            }
            else
            {
                await _roleRepository.UpdateAsync(role);
            }
        }

        public async ValueTask<bool> ExistPermissions(RolePermissionsCheckInputDto inputDto)
        {
            bool result = false;

            var codes = await this.GetPermissions(inputDto);

            if (codes != null && codes.Count() > 0)
                result = true;

            return result;
        }

        public async Task<IEnumerable<string>> GetPermissions(RolePermissionsCheckInputDto inputDto)
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
                return codes.Intersect(inputDto.Permissions.Select(x => x.ToUpper()));
            return null;
        }
    }
}
