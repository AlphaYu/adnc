using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using AutoMapper;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Infr.Common.Extensions;
using Adnc.Usr.Core.Entities;
using Adnc.Usr.Core.Services;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;

namespace Adnc.Usr.Application.Services
{
    public class UserAppService : AppService, IUserAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IDeptAppService _deptAppService;
        private readonly IRoleAppService _roleAppService;
        private readonly UsrManagerService _usrManager;

        public UserAppService(IMapper mapper,
            IEfRepository<SysUser> userRepository,
            IDeptAppService deptAppService,
            IRoleAppService roleAppService,
            UsrManagerService usrManager)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _deptAppService = deptAppService;
            _roleAppService = roleAppService;
            _usrManager = usrManager;
        }

        public async Task<AppSrvResult> ChangeStatus(long Id, int status)
        {
            await _userRepository.UpdateAsync(new SysUser { Id = Id, Status = status }, x => x.Status);
            return DefaultResult();
        }

        public async Task<AppSrvResult> ChangeStatus(UserChangeStatusInputDto changeDto)
        {
            string userids = string.Join<long>(",", changeDto.UserIds);
            await _userRepository.UpdateRangeAsync(u => userids.Contains(u.Id.ToString()), u => new SysUser { Status = changeDto.Status });
            return DefaultResult();
        }

        public async Task<AppSrvResult> Delete(long Id)
        {
            await _userRepository.DeleteAsync(Id);
            return DefaultResult();
        }

        public async Task<AppSrvResult<long>> Add(UserSaveInputDto saveDto)
        {
            if (await _userRepository.AnyAsync(x => x.Account == saveDto.Account))
                return Problem(HttpStatusCode.BadRequest, "账号已经存在");

            var user = _mapper.Map<SysUser>(saveDto);
            user.Id = IdGenerater.GetNextId();
            user.Salt = SecurityHelper.GenerateRandomCode(5);
            user.Password = HashHelper.GetHashedString(HashType.MD5, user.Password, user.Salt);
            await _usrManager.AddUser(user);

            return user.Id;
        }

        public async Task<AppSrvResult> Update(UserSaveInputDto saveDto)
        {
            var user = _mapper.Map<SysUser>(saveDto);
            await _userRepository.UpdateAsync(user,
                 x => x.Name,
                 x => x.DeptId,
                 x => x.Sex,
                 x => x.Phone,
                 x => x.Email,
                 x => x.Birthday,
                 x => x.Status);

            return DefaultResult();
        }

        public async Task<AppSrvResult<PageModelDto<UserDto>>> GetPaged(UserSearchDto searchDto)
        {
            Expression<Func<SysUser, bool>> whereCondition = x => true;
            if (searchDto.Account.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Account.Contains(searchDto.Account));
            }

            if (searchDto.Name.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(searchDto.Name));
            }

            var pagedModel = await _userRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, whereCondition, x => x.Id, false);
            var pageModelDto = _mapper.Map<PageModelDto<UserDto>>(pagedModel);

            pageModelDto.XData = await _deptAppService.GetSimpleList();

            if (pageModelDto.Count > 0)
            {
                var deptIds = pageModelDto.Data.Where(d => d.DeptId != null).Select(d => d.DeptId).Distinct().ToList();
                //var depts = await _deptRepository.SelectAsync(d => new { d.Id, d.FullName }, x => deptIds.Contains(x.Id));
                var depts = (await _deptAppService.GetAllFromCache())
                            .Where(x => deptIds.Contains(x.Id))
                            .Select(d => new { d.Id, d.FullName });
                //var roles = await _roleRepository.SelectAsync(r => new { r.Id, r.Name }, x => true);
                var roles = (await _roleAppService.GetAllFromCache())
                            .Select(r => new { r.Id, r.Name });

                foreach (var user in pageModelDto.Data)
                {
                    user.DeptName = depts.FirstOrDefault(x => x.Id == user.DeptId)?.FullName;
                    var roleIds = string.IsNullOrWhiteSpace(user.RoleId)
                        ? new List<long>()
                        : user.RoleId.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x))
                        ;
                    user.RoleName = string.Join(',', roles.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name));
                }
            }

            return pageModelDto;
        }

        public async Task<AppSrvResult> SetRole(RoleSetInputDto setDto)
        {
            var roleIdStr = setDto.RoleIds == null ? null : string.Join(",", setDto.RoleIds);
            await _userRepository.UpdateAsync(new SysUser() { Id = setDto.Id, RoleId = roleIdStr }, x => x.RoleId);

            return DefaultResult();
        }
    }
}
