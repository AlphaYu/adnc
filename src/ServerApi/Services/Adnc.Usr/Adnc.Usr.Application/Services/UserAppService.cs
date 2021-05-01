using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Helper;
using Adnc.Infra.Common.Extensions;
using Adnc.Usr.Core.Entities;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Application.Contracts.Services;
using Adnc.Usr.Application.Contracts.Consts;

namespace Adnc.Usr.Application.Services
{
    public class UserAppService : AbstractAppService, IUserAppService
    {
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IDeptAppService _deptAppService;
        private readonly CacheService _cacheService;

        public UserAppService(IEfRepository<SysUser> userRepository,
            IDeptAppService deptAppService,
            CacheService cacheService)
        {
            _userRepository = userRepository;
            _deptAppService = deptAppService;
            _cacheService = cacheService;
        }

        public async Task<AppSrvResult> ChangeStatusAsync(long id, int status)
        {
            await _userRepository.UpdateAsync(new SysUser { Id = id, Status = status }, UpdatingProps<SysUser>(x => x.Status));
            return AppSrvResult();
        }

        public async Task<AppSrvResult> ChangeStatusAsync(UserChangeStatusDto input)
        {
            string userids = string.Join<long>(",", input.UserIds);
            

            var cacheKeys = input.UserIds.Select(x => string.Format(EasyCachingConsts.UserLoginInfoKeyPrefix, x));
            await _cacheService.RemoveCachesAsync(cacheKeys, async () =>
            {
                await _userRepository.UpdateRangeAsync(u => userids.Contains(u.Id.ToString()), u => new SysUser { Status = input.Status });
            });
            return AppSrvResult();
        }

        public async Task<AppSrvResult> DeleteAsync(long id)
        {
            await _userRepository.DeleteAsync(id);
            return AppSrvResult();
        }

        public async Task<AppSrvResult<long>> CreateAsync(UserCreationDto input)
        {
            if (await _userRepository.AnyAsync(x => x.Account == input.Account))
                return Problem(HttpStatusCode.BadRequest, "账号已经存在");

            var user = Mapper.Map<SysUser>(input);
            user.Id = IdGenerater.GetNextId();
            user.Salt = SecurityHelper.GenerateRandomCode(5);
            user.Password = HashHelper.GetHashedString(HashType.MD5, user.Password, user.Salt);
            await _userRepository.InsertAsync(user);

            return user.Id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id,UserUpdationDto input)
        {
            var user = Mapper.Map<SysUser>(input);

            user.Id = id;

            var updatingProps = UpdatingProps<SysUser>(x => x.Name,
                                                       x => x.DeptId,
                                                       x => x.Sex,
                                                       x => x.Phone,
                                                       x => x.Email,
                                                       x => x.Birthday,
                                                       x => x.Status
                                                      );
            await _userRepository.UpdateAsync(user, updatingProps);

            return AppSrvResult();
        }

        public async Task<AppSrvResult> SetRoleAsync(long id, UserSetRoleDto input)
        {
            var roleIdStr = input.RoleIds == null ? null : string.Join(",", input.RoleIds);
            var cacheKeys = new string[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey, string.Format(EasyCachingConsts.UserLoginInfoKeyPrefix, id) };

            await _cacheService.RemoveCachesAsync(cacheKeys, async () =>
            {
                await _userRepository.UpdateAsync(new SysUser() { Id = id, RoleIds = roleIdStr }, UpdatingProps<SysUser>(x => x.RoleIds));
            });

            return AppSrvResult();
        }

        public async Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto search)
        {
            Expression<Func<SysUser, bool>> whereCondition = x => true;
            if (search.Account.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Account.Contains(search.Account));
            }

            if (search.Name.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(search.Name));
            }

            var pagedModel = await _userRepository.PagedAsync(search.PageIndex, search.PageSize, whereCondition, x => x.Id, false);
            var pageModelDto = Mapper.Map<PageModelDto<UserDto>>(pagedModel);

            if (pageModelDto.RowsCount > 0)
            {
                var deptIds = pageModelDto.Data.Where(d => d.DeptId != null).Select(d => d.DeptId).Distinct().ToList();
                var depts = (await _cacheService.GetAllDeptsFromCacheAsync())
                            .Where(x => deptIds.Contains(x.Id))
                            .Select(d => new { d.Id, d.FullName });
                var roles = (await _cacheService.GetAllRolesFromCacheAsync())
                            .Select(r => new { r.Id, r.Name });

                foreach (var user in pageModelDto.Data)
                {
                    user.DeptName = depts.FirstOrDefault(x => x.Id == user.DeptId)?.FullName;
                    var roleIds = string.IsNullOrWhiteSpace(user.RoleIds)
                        ? new List<long>()
                        : user.RoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x))
                        ;
                    user.RoleNames = string.Join(',', roles.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name));
                }
            }

            pageModelDto.XData = await _deptAppService.GetSimpleTreeListAsync();

            return pageModelDto;
        }

    }
}
