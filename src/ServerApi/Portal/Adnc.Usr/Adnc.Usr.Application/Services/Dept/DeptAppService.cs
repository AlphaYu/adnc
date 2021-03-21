using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EasyCaching.Core;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Core.Services;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared;
using System.Dynamic;

namespace Adnc.Usr.Application.Services
{
    public class DeptAppService : AppService, IDeptAppService
    {
        private readonly IMapper _mapper;
        private readonly IHybridCachingProvider _cache;
        private readonly IEasyCachingProvider _locaCahce;
        private readonly IEasyCachingProvider _redisCache;
        private readonly IEfRepository<SysDept> _deptRepository;
        private readonly UsrManager _usrManager;

        public DeptAppService(IMapper mapper
            , IHybridProviderFactory hybridProviderFactory
            , IEasyCachingProviderFactory simpleProviderFactory
            , IEfRepository<SysDept> deptRepository
            , UsrManager usrManager)
        {
            _mapper = mapper;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
            _locaCahce = simpleProviderFactory.GetCachingProvider(EasyCachingConsts.LocalCaching);
            _redisCache = simpleProviderFactory.GetCachingProvider(EasyCachingConsts.RemoteCaching);
            _deptRepository = deptRepository;
            _usrManager = usrManager;
        }

        public async Task<AppSrvResult> DeleteAsync(long Id)
        {
            //var depts1 = (await _locaCahce.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            //var depts2 = (await _redisCache.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            //var depts3 = (await _cache.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            var dept = await _deptRepository.FindAsync(Id);
            var deletingPids = $"{dept.Pids}[{Id}],";
            await _deptRepository.DeleteRangeAsync(d => d.Pids.StartsWith(deletingPids) || d.Id == dept.Id);

            return AppSrvResult();
        }

        public async Task<AppSrvResult<List<DeptTreeeDto>>> GetListAsync()
        {
            var result = new List<DeptTreeeDto>();

            var depts = await this.GetAllFromCacheAsync();
            if (!depts.Any())
                return result;

            var allDeptNodes = _mapper.Map<List<DeptTreeeDto>>(depts);

            var roots = allDeptNodes.Where(d => d.Pid == 0).OrderBy(d => d.Ordinal);
            foreach (var node in roots)
            {
                GetChildren(node, allDeptNodes);
                result.Add(node);
            }

            void GetChildren(DeptTreeeDto currentNode, List<DeptTreeeDto> allDeptNodes)
            {
                var childrenNodes = allDeptNodes.Where(d => d.Pid == currentNode.Id).OrderBy(d => d.Ordinal);
                if (childrenNodes.Count() == 0)
                    return;
                else
                {
                    currentNode.Children.AddRange(childrenNodes);
                    foreach(var node in childrenNodes)
                    {
                        GetChildren(node, allDeptNodes);
                    }
                }
            }
            return result;
        }

        public async Task<AppSrvResult<long>> CreateAsync(DeptCreationDto input)
        {
            var isExists = (await GetAllFromCacheAsync()).Exists(x => x.FullName == input.FullName);
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该部门全称已经存在");

            var dept = _mapper.Map<SysDept>(input);
            dept.Id = IdGenerater.GetNextId();
            await this.SetDeptPids(dept);
            await _deptRepository.InsertAsync(dept);

            return dept.Id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id, DeptUpdationDto input)
        {
            var oldDeptDto = (await GetAllFromCacheAsync()).FirstOrDefault(x => x.Id == id);
            if (oldDeptDto.Pid == 0 && input.Pid > 0)
                return Problem(HttpStatusCode.BadRequest, "一级单位不能修改等级");

            var isExists = (await GetAllFromCacheAsync()).Exists(x => x.FullName == input.FullName && x.Id != id);
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该部门全称已经存在");

            var oldPids = oldDeptDto.Pids;

            var deptEnity = _mapper.Map<SysDept>(input);

            deptEnity.Id = id;

            if (oldDeptDto.Pid == input.Pid)
            {
                await _deptRepository.UpdateAsync(deptEnity, UpdatingProps<SysDept>(x => x.FullName, x => x.SimpleName, x => x.Tips, x => x.Ordinal));
            }
            else
            {
                await this.SetDeptPids(deptEnity);
                await _usrManager.UpdateDeptAsync(oldPids, deptEnity);
            }

            return AppSrvResult();
        }

        private async Task<SysDept> SetDeptPids(SysDept sysDept)
        {

            if (sysDept.Pid.HasValue && sysDept.Pid.Value > 0)
            {
                var dept = (await GetAllFromCacheAsync()).FirstOrDefault(x => x.Id == sysDept.Pid.Value);
                string pids = dept?.Pids ?? "";
                sysDept.Pids = $"{pids}[{sysDept.Pid}],";
            }
            else
            {
                sysDept.Pid = 0;
                sysDept.Pids = "[0],";
            }
            return sysDept;
        }

        public async Task<List<DeptDto>> GetAllFromCacheAsync()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.DetpListCacheKey, async() =>
            {
                var allDepts = await _deptRepository.GetAll(writeDb:true).OrderBy(x=>x.Ordinal).ToListAsync();
                return _mapper.Map<List<DeptDto>>(allDepts);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        public async Task<dynamic[]> GetSimpleListAsync()
        {
            var depts = (await this.GetListAsync()).Content;
            if (!depts.Any())
                return default(dynamic[]);

            return GetSimpleNodes(depts);

            dynamic[] GetSimpleNodes(List<DeptTreeeDto> deptNodes)
            {
                var result = new dynamic[deptNodes.Count];

                for (var index = 0; index < deptNodes.Count; index++)
                {
                    dynamic simpleNode = new ExpandoObject();
                    simpleNode.Id = deptNodes[index].Id;
                    simpleNode.Label = deptNodes[index].SimpleName;
                    if (deptNodes[index].Children.Any())
                        simpleNode.children = GetSimpleNodes(deptNodes[index].Children);
                    result[index] = simpleNode;
                }
                return result;
            }
        }
    }
}
