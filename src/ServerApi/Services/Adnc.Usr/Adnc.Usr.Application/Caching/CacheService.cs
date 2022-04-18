using Adnc.Application.Shared.Caching;
using Adnc.Infra.Caching;
using Adnc.Infra.Helper;
using Adnc.Infra.IRepositories;
using Adnc.Shared.ConfigModels;
using Adnc.Shared.Consts.Caching.Usr;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.Caching
{
    public class CacheService : AbstractCacheService
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IEfRepository<SysDept>> _deptRepository;
        private readonly Lazy<IEfRepository<SysMenu>> _menuRepository;
        private readonly Lazy<IEfRepository<SysRelation>> _relationRepository;
        private readonly Lazy<IEfRepository<SysRole>> _roleRepository;
        private readonly Lazy<IEfRepository<SysUser>> _userRepository;
        private readonly Lazy<IOptionsSnapshot<JwtConfig>> _jwtConfig;

        public CacheService(Lazy<ICacheProvider> cache
            , Lazy<IEfRepository<SysDept>> deptRepository
            , Lazy<IEfRepository<SysMenu>> menuRepository
            , Lazy<IEfRepository<SysRelation>> relationRepository
            , Lazy<IEfRepository<SysRole>> roleRepository
            , Lazy<IEfRepository<SysUser>> userRepository
            ,Lazy<IOptionsSnapshot<JwtConfig>> jwtConfig)
            : base(cache)
        {
            _cache = cache;
            _deptRepository = deptRepository;
            _menuRepository = menuRepository;
            _relationRepository = relationRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _jwtConfig = jwtConfig;
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
                                                                                            .Where(x => x.Menu.Status)
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
                    Id = x.Id,
                    Account = x.Account,
                    Status = x.Status,
                    Name = x.Name,
                    RoleIds = x.RoleIds,
                    ValidationVersion = HashHelper.GetHashedString(HashType.MD5, x.Account + x.Password)
                }, x => x.Id == Id);
            }, TimeSpan.FromSeconds(_jwtConfig.Value.Value.Expire * 60 + _jwtConfig.Value.Value.ClockSkew));

            return cacheValue.Value;
        }

        internal async Task<List<DeptSimpleTreeDto>> GetDeptSimpleTreeListAsync()
        {
            var result = new List<DeptSimpleTreeDto>();

            var cacheValue = await _cache.Value.GetAsync<List<DeptSimpleTreeDto>>(CachingConsts.DetpSimpleTreeListCacheKey);
            if (cacheValue.HasValue)
                return cacheValue.Value;

            var depts = await GetAllDeptsFromCacheAsync();

            if (depts.IsNullOrEmpty())
                return result;

            var roots = depts.Where(d => d.Pid == 0)
                                        .OrderBy(d => d.Ordinal)
                                        .Select(x => new DeptSimpleTreeDto { Id = x.Id, Label = x.SimpleName})
                                        .ToList();
            foreach (var node in roots)
            {
                GetChildren(node, depts);
                result.Add(node);
            }

            void GetChildren(DeptSimpleTreeDto currentNode, List<DeptDto> depts)
            {
                var childrenNodes = depts.Where(d => d.Pid == currentNode.Id)
                                                           .OrderBy(d => d.Ordinal)
                                                           .Select(x => new DeptSimpleTreeDto() { Id = x.Id, Label = x.SimpleName })
                                                           .ToList();
                if (childrenNodes.IsNotNullOrEmpty())
                {
                    currentNode.Children.AddRange(childrenNodes);
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