using AutoMapper;
using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Common;
using Adnc.Common.Models;
using Adnc.Core.CoreServices;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;
using Adnc.Common.Helper;
using Microsoft.EntityFrameworkCore;

namespace Adnc.Application.Services
{
    public class DeptAppService : AppService, IDeptAppService
    {
        private readonly IMapper _mapper;
        private readonly IHybridCachingProvider _cache;
        private readonly IEasyCachingProvider _locaCahce;
        private readonly IEasyCachingProvider _redisCache;
        private readonly IEfRepository<SysDept> _deptRepository;
        private readonly ISystemManagerService _systemManagerService;

        public DeptAppService(IMapper mapper
            , IHybridProviderFactory hybridProviderFactory
            , IEasyCachingProviderFactory simpleProviderFactory
            , IEfRepository<SysDept> deptRepository
            , ISystemManagerService systemManagerService)
        {
            _mapper = mapper;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
            _locaCahce = simpleProviderFactory.GetCachingProvider(EasyCachingConsts.LocalCaching);
            _redisCache = simpleProviderFactory.GetCachingProvider(EasyCachingConsts.RemoteCaching);
            _deptRepository = deptRepository;
            _systemManagerService = systemManagerService;
        }

        public async Task Delete(long Id)
        {
            //var depts1 = (await _locaCahce.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            //var depts2 = (await _redisCache.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;
            //var depts3 = (await _cache.GetAsync<List<DeptNodeDto>>(EasyCachingConsts.DetpListCacheKey)).Value;

            await _systemManagerService.DeleteDept(Id);
        }

        public async Task<List<DeptNodeDto>> GetList()
        {
            var result = new List<DeptNodeDto>();

            var depts = await this.GetAll();
            if (depts.Any())
            {
                var deptNodes = _mapper.Map<List<DeptNodeDto>>(depts);
                var dictDepts = deptNodes.ToDictionary(x => x.ID);
                foreach (var pair in dictDepts)
                {
                    var currentDept = pair.Value;
                    var parentDept = deptNodes.FirstOrDefault(x => x.ID == currentDept.Pid);
                    if (parentDept != null)
                    {
                        parentDept.Children.Add(currentDept);
                    }
                    else
                    {
                        result.Add(currentDept);
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
                dept.ID = IdGeneraterHelper.GetNextId(IdGeneraterKey.DEPT);
                await this.SetDeptPids(dept);
                await _deptRepository.InsertAsync(dept);
            }
            else
            {
                var oldDept = await _deptRepository.FetchAsync(d => d, x => x.ID == saveDto.ID);
                oldDept.Pid = saveDto.Pid;
                oldDept.SimpleName = saveDto.SimpleName;
                oldDept.FullName = saveDto.FullName;
                oldDept.Num = saveDto.Num;
                oldDept.Tips = saveDto.Tips;

                var dept = await this.SetDeptPids(oldDept);

                await _deptRepository.UpdateAsync(dept);
            }
        }

        private async Task<SysDept> SetDeptPids(SysDept sysDept)
        {
            if (sysDept.Pid.HasValue && sysDept.Pid.Value > 0)
            {
                var depts = await this.GetAll();
                var dept = depts.Select(d => new { d.ID, d.Pid, d.Pids }).Where(d => d.ID == sysDept.Pid.Value).FirstOrDefault();
                //var dept = await _deptRepository.FetchAsync(d => new { d.ID, d.Pid, d.Pids }, x => x.ID == sysDept.Pid.Value);
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
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.DetpListCacheKey, async () =>
            {
                var allDepts = await _deptRepository.GetAll().ToListAsync();
                return _mapper.Map<List<DeptDto>>(allDepts);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }
    }
}
