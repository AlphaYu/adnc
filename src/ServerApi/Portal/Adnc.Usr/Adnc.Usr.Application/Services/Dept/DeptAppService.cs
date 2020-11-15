using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EasyCaching.Core;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Core.CoreServices;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Microsoft.EntityFrameworkCore;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared;

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

        public async Task Delete(long Id)
        {
            //var depts1 = (await _locaCahce.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            //var depts2 = (await _redisCache.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            //var depts3 = (await _cache.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            var dept = await _deptRepository.FindAsync(new object[] { Id });
            var deletingPids = $"{dept.Pids}[{Id}],";
            await _deptRepository.DeleteRangeAsync(d => d.Pids.StartsWith(deletingPids) || d.ID == dept.ID);
        }

        public async Task<List<DeptNodeDto>> GetList()
        {
            var result = new List<DeptNodeDto>();

            var depts = await this.GetAll();
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

        public async Task Save(DeptSaveInputDto saveDto)
        {
            if (string.IsNullOrWhiteSpace(saveDto.FullName))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.BadRequest, "请输入部门全称"));
            }

            if (string.IsNullOrWhiteSpace(saveDto.SimpleName))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.BadRequest,"请输入部门简称"));
            }

            if (saveDto.ID == 0)
            {
                var dept = _mapper.Map<SysDept>(saveDto);
                dept.ID = IdGenerater.GetNextId();
                await this.SetDeptPids(dept);
                await _deptRepository.InsertAsync(dept);
            }
            else
            {
                var dept = await _deptRepository.FetchAsync(d => d, x => x.ID == saveDto.ID);
                if (dept.Pid == 0 && saveDto.Pid > 0)
                    throw new BusinessException(new ErrorModel(ErrorCode.BadRequest, "一级单位不能修改等级"));

                var oldPids = dept.Pids;

                dept.SimpleName = saveDto.SimpleName;
                dept.FullName = saveDto.FullName;
                dept.Num = saveDto.Num;
                dept.Tips = saveDto.Tips;

                if (dept.Pid == saveDto.Pid)
                {
                    await _deptRepository.UpdateAsync(dept);
                }
                else
                {
                    dept.Pid = saveDto.Pid;
                    await this.SetDeptPids(dept);
                    await _usrManagerService.UpdateDept(oldPids, dept);
                }
                //var subDetps = _deptRepository.GetAll().Where(d => d.Pids.Contains($"[{dept.ID}]")).ToList();
                //subDetps.ForEach(c =>
                //{
                //    c.Pids = c.Pids.Replace(oldPids,dept.Pids);
                //});    
            }
        }

        private async Task<SysDept> SetDeptPids(SysDept sysDept)
        {

            if (sysDept.Pid.HasValue && sysDept.Pid.Value > 0)
            {
                var depts = await this.GetAll();
                //var dept = depts.Select(d => new { d.ID, d.Pid, d.Pids }).Where(d => d.ID == sysDept.Pid.Value).FirstOrDefault();
                var dept = await _deptRepository.FetchAsync(d => new { d.ID, d.Pid, d.Pids }, x => x.ID == sysDept.Pid.Value);
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

        private async Task<List<DeptDto>> GetAll()
        {
            var tempcache = await _cache.GetAsync<List<DeptDto>>(EasyCachingConsts.DetpListCacheKey);

            var cahceValue = await _cache.GetAsync(EasyCachingConsts.DetpListCacheKey, async() =>
            {
                var allDepts = await _deptRepository.GetAll().ToListAsync();
                return _mapper.Map<List<DeptDto>>(allDepts);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }
    }
}
