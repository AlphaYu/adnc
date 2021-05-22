using Adnc.Application.Shared.Caching;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Caching;
using Adnc.Usr.Application.Contracts.Consts;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.Caching
{
    public class CacheService : AbstractCacheService
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IDistributedLocker> _distributedLocker;
        private readonly Lazy<IRedisProvider> _redisProvider;
        private readonly Lazy<IEfRepository<SysDept>> _deptRepository;
        private readonly Lazy<IEfRepository<SysMenu>> _menuRepository;
        private readonly Lazy<IEfRepository<SysRelation>> _relationRepository;
        private readonly Lazy<IEfRepository<SysRole>> _roleRepository;
        private readonly Lazy<IEfRepository<SysUser>> _userRepository;
        private readonly Lazy<IBloomFilterFactory> _bloomFilterFactory;

        public CacheService(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker
            , Lazy<IEfRepository<SysDept>> deptRepository
            , Lazy<IEfRepository<SysMenu>> menuRepository
            , Lazy<IEfRepository<SysRelation>> relationRepository
            , Lazy<IEfRepository<SysRole>> roleRepository
            , Lazy<IEfRepository<SysUser>> userRepository
            , Lazy<IBloomFilterFactory> bloomFilterFactory)
            : base(cache, redisProvider, distributedLocker)
        {
            _cache = cache;
            _redisProvider = redisProvider;
            _distributedLocker = distributedLocker;
            _deptRepository = deptRepository;
            _menuRepository = menuRepository;
            _relationRepository = relationRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _bloomFilterFactory = bloomFilterFactory;
        }

        public override async Task PreheatAsync()
        {
            await GetAllDeptsFromCacheAsync();
            await GetAllRelationsFromCacheAsync();
            await GetAllMenusFromCacheAsync();
            await GetAllRolesFromCacheAsync();
            await GetAllMenuCodesFromCacheAsync();
            await GetDeptSimpleTreeListAsync();
        }

        internal (IBloomFilter CacheKeys, IBloomFilter Accounts) BloomFilters
        {
            get
            {
                var cacheFilter = _bloomFilterFactory.Value.GetBloomFilter(_cache.Value.CacheOptions.PenetrationSetting.BloomFilterSetting.Name);
                var accountFilter = _bloomFilterFactory.Value.GetBloomFilter($"adnc:{nameof(BloomFilterAccount).ToLower()}");
                return (cacheFilter, accountFilter);
            }
        }

        internal async Task SetValidateInfoToCacheAsync(UserValidateDto value)
        {
            var cacheKey = ConcatCacheKey(CachingConsts.UserValidateInfoKeyPrefix, value.Id);
            await _cache.Value.SetAsync(cacheKey, value, TimeSpan.FromSeconds(CachingConsts.OneDay));
        }

        internal async Task<List<DeptDto>> GetAllDeptsFromCacheAsync()
        {
            var cahceValue = await _cache.Value.GetAsync(CachingConsts.DetpListCacheKey, async () =>
            {
                var allDepts = await _deptRepository.Value.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
                return Mapper.Map<List<DeptDto>>(allDepts);
            }, TimeSpan.FromSeconds(CachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<List<RelationDto>> GetAllRelationsFromCacheAsync()
        {
            var cahceValue = await _cache.Value.GetAsync(CachingConsts.MenuRelationCacheKey, async () =>
            {
                var allRelations = await _relationRepository.Value.GetAll(writeDb: true).ToListAsync();
                return Mapper.Map<List<RelationDto>>(allRelations);
            }, TimeSpan.FromSeconds(CachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<List<MenuDto>> GetAllMenusFromCacheAsync()
        {
            var cahceValue = await _cache.Value.GetAsync(CachingConsts.MenuListCacheKey, async () =>
            {
                var allMenus = await _menuRepository.Value.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
                return Mapper.Map<List<MenuDto>>(allMenus);
            }, TimeSpan.FromSeconds(CachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<List<RoleDto>> GetAllRolesFromCacheAsync()
        {
            var cahceValue = await _cache.Value.GetAsync(CachingConsts.RoleListCacheKey, async () =>
            {
                var allRoles = await _roleRepository.Value.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
                return Mapper.Map<List<RoleDto>>(allRoles);
            }, TimeSpan.FromSeconds(CachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<List<RoleMenuCodesDto>> GetAllMenuCodesFromCacheAsync()
        {
            var cahceValue = await _cache.Value.GetAsync(CachingConsts.MenuCodesCacheKey, async () =>
            {
                var allMenus = await _relationRepository.Value.GetAll(writeDb: true)
               .Where(x => x.Menu.Status == true)
               .Select(x => new RoleMenuCodesDto { RoleId = x.RoleId, Code = x.Menu.Code })
               .ToListAsync();
                return allMenus.Distinct().ToList();
            }, TimeSpan.FromSeconds(CachingConsts.OneYear));

            return cahceValue.Value;
        }

        internal async Task<UserValidateDto> GetUserValidateInfoFromCacheAsync(long Id)
        {
            var cacheKey = ConcatCacheKey(CachingConsts.UserValidateInfoKeyPrefix, Id.ToString());

            var cacheValue = await _cache.Value.GetAsync(cacheKey, async () =>
            {
                return await _userRepository.Value.FetchAsync(x => new UserValidateDto()
                {
                    Id = x.Id
                   ,
                    Account = x.Account
                    ,
                    Password = x.Password
                   ,
                    Salt = x.Salt
                   ,
                    Status = x.Status
                   ,
                    Email = x.Email
                   ,
                    Name = x.Name
                   ,
                    RoleIds = x.RoleIds
                }, x => x.Id == Id);
            }, TimeSpan.FromSeconds(CachingConsts.OneDay));

            return cacheValue.Value;
        }

        internal async Task<List<DeptSimpleTreeDto>> GetDeptSimpleTreeListAsync()
        {
            var result = new List<DeptSimpleTreeDto>();

            var depts = await GetAllDeptsFromCacheAsync();

            if (!depts.Any())
                return result;

            var roots = depts.Where(d => d.Pid == 0)
                                                      .OrderBy(d => d.Ordinal)
                                                      .Select(x => new DeptSimpleTreeDto() { Id = x.Id, Label = x.SimpleName, children = new List<DeptSimpleTreeDto>() });
            foreach (var node in roots)
            {
                GetChildren(node, depts);
                result.Add(node);
            }

            void GetChildren(DeptSimpleTreeDto currentNode, List<DeptDto> depts)
            {
                var childrenNodes = depts.Where(d => d.Pid == currentNode.Id)
                                                                         .OrderBy(d => d.Ordinal)
                                                                         .Select(x => new DeptSimpleTreeDto() { Id = x.Id, Label = x.SimpleName, children = new List<DeptSimpleTreeDto>() });
                if (childrenNodes.Count() == 0)
                    return;
                else
                {
                    currentNode.children.AddRange(childrenNodes);
                    foreach (var node in childrenNodes)
                    {
                        GetChildren(node, depts);
                    }
                }
            }

            await _cache.Value.SetAsync(CachingConsts.DetpSimpleTreeListCacheKey, result, TimeSpan.FromSeconds(CachingConsts.OneYear));

            return result;
        }
    }
}