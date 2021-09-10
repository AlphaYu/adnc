using Adnc.Application.Shared.BloomFilter;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Helper;
using Adnc.Infra.IRepositories;
using Adnc.Shared.Consts.Caching.Usr;
using Adnc.Usr.Application.BloomFilter;
using Adnc.Usr.Application.Caching;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Application.Contracts.Services;
using Adnc.Usr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.Services
{
    public class UserAppService : AbstractAppService, IUserAppService
    {
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly CacheService _cacheService;
        private readonly IBloomFilterFactory _bloomFilterFactory;

        public UserAppService(IEfRepository<SysUser> userRepository
            , CacheService cacheService
           , IBloomFilterFactory bloomFilterFactory)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
            _bloomFilterFactory = bloomFilterFactory;
        }

        public async Task<AppSrvResult<long>> CreateAsync(UserCreationDto input)
        {
            if (await _userRepository.AnyAsync(x => x.Account == input.Account))
                return Problem(HttpStatusCode.BadRequest, "账号已经存在");

            var user = Mapper.Map<SysUser>(input);
            user.Id = IdGenerater.GetNextId();
            user.Account = user.Account.ToLower();
            user.Salt = SecurityHelper.GenerateRandomCode(5);
            user.Password = HashHelper.GetHashedString(HashType.MD5, user.Password, user.Salt);

            var cacheKey = _cacheService.ConcatCacheKey(CachingConsts.UserValidateInfoKeyPrefix, user.Id);
            var bloomFilterCacheKey = _bloomFilterFactory.GetBloomFilter(nameof(BloomFilterCacheKey));
            await bloomFilterCacheKey.AddAsync(cacheKey);

            var bloomFilterAccount = _bloomFilterFactory.GetBloomFilter(nameof(BloomFilterAccount));
            await bloomFilterAccount.AddAsync(user.Account);

            await _userRepository.InsertAsync(user);

            return user.Id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id, UserUpdationDto input)
        {
            var user = Mapper.Map<SysUser>(input);

            user.Id = id;
            var updatingProps = UpdatingProps<SysUser>(x => x.Name, x => x.DeptId, x => x.Sex, x => x.Phone, x => x.Email, x => x.Birthday, x => x.Status);
            await _userRepository.UpdateAsync(user, updatingProps);

            return AppSrvResult();
        }

        public async Task<AppSrvResult> SetRoleAsync(long id, UserSetRoleDto input)
        {
            var roleIdStr = input.RoleIds == null ? null : string.Join(",", input.RoleIds);
            await _userRepository.UpdateAsync(new SysUser() { Id = id, RoleIds = roleIdStr }, UpdatingProps<SysUser>(x => x.RoleIds));

            return AppSrvResult();
        }

        public async Task<AppSrvResult> DeleteAsync(long id)
        {
            await _userRepository.DeleteAsync(id);
            return AppSrvResult();
        }

        public async Task<AppSrvResult> ChangeStatusAsync(long id, int status)
        {
            await _userRepository.UpdateAsync(new SysUser { Id = id, Status = status }, UpdatingProps<SysUser>(x => x.Status));
            return AppSrvResult();
        }

        public async Task<AppSrvResult> ChangeStatusAsync(IEnumerable<long> ids, int status)
        {
            string userids = string.Join(",", ids);
            await _userRepository.UpdateRangeAsync(u => userids.Contains(u.Id.ToString()), u => new SysUser { Status = status });
            return AppSrvResult();
        }

        public async Task<List<string>> GetPermissionsAsync(long userId, IEnumerable<string> permissions, string validationVersion = null)
        {
            var userValidateInfo = await _cacheService.GetUserValidateInfoFromCacheAsync(userId);
            if (userValidateInfo.RoleIds.IsNullOrWhiteSpace())
                return default;

            if (userValidateInfo.Status != 1)
                return default;

            if (validationVersion.IsNotNullOrWhiteSpace() && userValidateInfo.ValidationVersion != validationVersion)
                return default;

            var roleIds = userValidateInfo.RoleIds.Trim().Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));

            var allMenuCodes = await _cacheService.GetAllMenuCodesFromCacheAsync();

            var codes = allMenuCodes?.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.Code.ToUpper());
            if (codes.IsNotNullOrEmpty())
            {
                var result = codes.Intersect(permissions.Select(x => x.ToUpper()));
                return result.ToList();
            }

            return default;
        }

        public async Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto search)
        {
            var whereCondition = ExpressionCreator
                                                                            .New<SysUser>()
                                                                            .AndIf(search.Account.IsNotNullOrWhiteSpace(), x => x.Account.Contains(search.Account))
                                                                            .AndIf(search.Name.IsNotNullOrWhiteSpace(), x => x.Name.Contains(search.Name));

            var pagedModel = await _userRepository.PagedAsync(search.PageIndex, search.PageSize, whereCondition, x => x.Id, false);
            var pageModelDto = Mapper.Map<PageModelDto<UserDto>>(pagedModel);

            if (pageModelDto.RowsCount > 0)
            {
                var deptIds = pageModelDto.Data.Where(d => d.DeptId != null)
                                                                        .Select(d => d.DeptId)
                                                                        .Distinct();

                var depts = (await _cacheService.GetAllDeptsFromCacheAsync())
                                                                    .Where(x => deptIds.Contains(x.Id))
                                                                    .Select(d => new { d.Id, d.FullName });

                var roles = (await _cacheService.GetAllRolesFromCacheAsync())
                                                                   .Select(r => new { r.Id, r.Name });

                foreach (var user in pageModelDto.Data)
                {
                    user.DeptName = depts.FirstOrDefault(x => x.Id == user.DeptId)?.FullName;
                    var roleIds = user.RoleIds.IsNullOrWhiteSpace()
                                                                                                ? new List<long>()
                                                                                                : user.RoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x))
                                                                                                ;

                    user.RoleNames = roles.Where(x => roleIds.Contains(x.Id))
                                                          .Select(x => x.Name)
                                                          .ToString(",");
                }
            }

            pageModelDto.XData = await _cacheService.GetDeptSimpleTreeListAsync();

            return pageModelDto;
        }
    }
}