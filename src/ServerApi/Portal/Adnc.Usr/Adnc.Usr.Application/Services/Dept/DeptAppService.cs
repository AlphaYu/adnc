using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using EasyCaching.Core;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Core.CoreServices;
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
        private readonly IUsrManagerService _usrManagerService;

        public DeptAppService(IMapper mapper
            , IHybridProviderFactory hybridProviderFactory
            , IEasyCachingProviderFactory simpleProviderFactory
            , IEfRepository<SysDept> deptRepository
            , IUsrManagerService usrManagerService)
        {
            _mapper = mapper;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
            _locaCahce = simpleProviderFactory.GetCachingProvider(EasyCachingConsts.LocalCaching);
            _redisCache = simpleProviderFactory.GetCachingProvider(EasyCachingConsts.RemoteCaching);
            _deptRepository = deptRepository;
            _usrManagerService = usrManagerService;
        }

        public async Task<AppSrvResult> Delete(long Id)
        {
            //var depts1 = (await _locaCahce.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            //var depts2 = (await _redisCache.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            //var depts3 = (await _cache.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            var dept = await _deptRepository.FindAsync(Id);
            var deletingPids = $"{dept.Pids}[{Id}],";
            await _deptRepository.DeleteRangeAsync(d => d.Pids.StartsWith(deletingPids) || d.ID == dept.ID);

            return DefaultResult();
        }

        public async Task<AppSrvResult<List<DeptNodeDto>>> GetList()
        {
            var result = new List<DeptNodeDto>();

            var depts = await this.GetAllFromCache();
            if (!depts.Any())
                return result;

            var allDeptNodes = _mapper.Map<List<DeptNodeDto>>(depts);

            var roots = allDeptNodes.Where(d => d.Pid == 0).OrderBy(d => d.Num);
            foreach (var node in roots)
            {
                GetChildren(node, allDeptNodes);
                result.Add(node);
            }

            void GetChildren(DeptNodeDto currentNode, List<DeptNodeDto> allDeptNodes)
            {
                var childrenNodes = allDeptNodes.Where(d => d.Pid == currentNode.ID).OrderBy(d => d.Num);
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

        public async Task<AppSrvResult<long>> Add(DeptSaveInputDto saveDto)
        {
            var isExists = (await GetAllFromCache()).Exists(x => x.FullName == saveDto.FullName);
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该部门全称已经存在");

            var dept = _mapper.Map<SysDept>(saveDto);
            dept.ID = IdGenerater.GetNextId();
            await this.SetDeptPids(dept);
            await _deptRepository.InsertAsync(dept);

            return dept.ID;
        }

        public async Task<AppSrvResult> Update(DeptSaveInputDto saveDto)
        {
            var oldDeptDto = (await GetAllFromCache()).FirstOrDefault(x => x.ID == saveDto.ID);
            if (oldDeptDto.Pid == 0 && saveDto.Pid > 0)
                return Problem(HttpStatusCode.BadRequest, "一级单位不能修改等级");

            var isExists = (await GetAllFromCache()).Exists(x => x.FullName == saveDto.FullName && x.ID != saveDto.ID);
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该部门全称已经存在");

            var oldPids = oldDeptDto.Pids;

            var deptEnity = _mapper.Map<SysDept>(saveDto);

            if (oldDeptDto.Pid == saveDto.Pid)
            {
                await _deptRepository.UpdateAsync(deptEnity);
            }
            else
            {
                await this.SetDeptPids(deptEnity);
                await _usrManagerService.UpdateDept(oldPids, deptEnity);
            }

            return DefaultResult();
        }

        private async Task<SysDept> SetDeptPids(SysDept sysDept)
        {

            if (sysDept.Pid.HasValue && sysDept.Pid.Value > 0)
            {
                //var depts = await this.GetAllFromCache();
                //var dept = depts.Select(d => new { d.ID, d.Pid, d.Pids }).Where(d => d.ID == sysDept.Pid.Value).FirstOrDefault();
                var dept = (await GetAllFromCache()).FirstOrDefault(x => x.ID == sysDept.Pid.Value);
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

        public async Task<List<DeptDto>> GetAllFromCache()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.DetpListCacheKey, async() =>
            {
                var allDepts = await _deptRepository.GetAll(writeDb:true).ToListAsync();
                return _mapper.Map<List<DeptDto>>(allDepts);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        public async Task<dynamic[]> GetSimpleList()
        {
            var depts = (await this.GetList()).Content;
            if (!depts.Any())
                return default(dynamic[]);

            return GetSimpleNodes(depts);

            dynamic[] GetSimpleNodes(List<DeptNodeDto> deptNodes)
            {
                var result = new dynamic[deptNodes.Count];

                for (var index = 0; index < deptNodes.Count; index++)
                {
                    dynamic simpleNode = new ExpandoObject();
                    simpleNode.Id = deptNodes[index].ID;
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
