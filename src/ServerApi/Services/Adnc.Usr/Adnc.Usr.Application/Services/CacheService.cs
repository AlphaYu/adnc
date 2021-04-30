using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adnc.Application.Shared.Services;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Caching;
using Adnc.Usr.Application.Contracts.Consts;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Core.Entities;

namespace Adnc.Usr.Application.Services
{
    public class CacheService : AbstractCacheService
    {
        private readonly IRedisDistributedCache _cache;
        private readonly Lazy<IRedisProvider> _redisProvider;
        private readonly Lazy<IEfRepository<SysDept>> _deptRepository;
        private readonly Lazy<IEfRepository<SysMenu>> _menuRepository;
        private readonly Lazy<IEfRepository<SysRelation>> _relationRepository;
        private readonly Lazy<IEfRepository<SysRole>> _roleRepository;
        private readonly Lazy<IEfRepository<SysUser>> _userRepository;

        public CacheService(IRedisDistributedCache cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IEfRepository<SysDept>> deptRepository
            , Lazy<IEfRepository<SysMenu>> menuRepository
            , Lazy<IEfRepository<SysRelation>> relationRepository
            , Lazy<IEfRepository<SysRole>> roleRepository
            , Lazy<IEfRepository<SysUser>> userRepository)
            : base(cache, redisProvider)
        {
            _cache = cache;
            _deptRepository = deptRepository;
            _menuRepository = menuRepository;
            _relationRepository = relationRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        internal async Task<List<DeptDto>> GetAllDeptsFromCacheAsync()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.DetpListCacheKey, async () =>
            {
                var allDepts = await _deptRepository.Value.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
                return Mapper.Map<List<DeptDto>>(allDepts);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<List<RelationDto>> GetAllRelationsFromCacheAsync()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuRelationCacheKey, async () =>
            {
                var allRelations = await _relationRepository.Value.GetAll(writeDb: true).ToListAsync();
                return Mapper.Map<List<RelationDto>>(allRelations);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<List<MenuDto>> GetAllMenusFromCacheAsync()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuListCacheKey, async () =>
            {
                var allMenus = await _menuRepository.Value.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
                return Mapper.Map<List<MenuDto>>(allMenus);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<List<RoleDto>> GetAllRolesFromCacheAsync()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.RoleAllCacheKey, async () =>
            {
                var allRoles = await _roleRepository.Value.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
                return Mapper.Map<List<RoleDto>>(allRoles);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<List<RoleMenuCodesDto>> GetAllMenuCodesFromCache()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuCodesCacheKey, async () =>
            {
                var allMenus = await _relationRepository.Value.GetAll(writeDb: true)
               .Where(x => x.Menu.Status == true)
               .Select(x => new RoleMenuCodesDto { RoleId = x.RoleId, Code = x.Menu.Code })
               .ToListAsync();
                return allMenus.Distinct().ToList();
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<UserValidateDto> GetUserValidateInfoFromCache(string account)
        {
            var cacheKey = string.Format(EasyCachingConsts.UserLoginInfoKey, account.ToLower());

            var cacheValue = await _cache.GetAsync(cacheKey, async () =>
            {
                return await _userRepository.Value.FetchAsync(x => new UserValidateDto()
                {
                    Password = x.Password
                   ,
                    Salt = x.Salt
                   ,
                    Status = x.Status
                   ,
                    Account = x.Account
                   ,
                    Email = x.Email
                   ,
                    Id = x.Id
                   ,
                    Name = x.Name
                   ,
                    RoleIds = x.RoleIds
                }, x => x.Account == account);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneWeek));

            return cacheValue.Value;
        }
    }
}
