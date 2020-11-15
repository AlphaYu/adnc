using AutoMapper;
using System.Threading.Tasks;
using Adnc.Core.Shared.IRepositories;
using Adnc.Usr.Application.Dtos;
using System.Linq.Expressions;
using System.Linq;
using System;
using Adnc.Infr.Common.Extensions;
using Adnc.Usr.Core.Entities;
using System.Collections.Generic;
using Adnc.Infr.Common.Helper;
using Adnc.Usr.Core.CoreServices;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared;

namespace Adnc.Usr.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IEfRepository<SysDept> _deptRepository;
        private readonly IEfRepository<SysRole> _roleRepository;
        private readonly IUsrManagerService _usrManager;

        public UserAppService(IMapper mapper,
            IEfRepository<SysUser> userRepository,
            IEfRepository<SysDept> deptRepository,
            IEfRepository<SysRole> roleRepository,
            IUsrManagerService usrManager)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _deptRepository = deptRepository;
            _roleRepository = roleRepository;
            _usrManager = usrManager;
        }

        public async Task ChangeStatus(long Id)
        {
            var user = await _userRepository.FetchAsync(u => new { u.ID, u.Status }, x => x.ID == Id);
            user.Status = (int)(user.Status == (int)ManageStatus.Enabled ? ManageStatus.Disabled : ManageStatus.Enabled);

            await _userRepository.UpdateAsync(user, x => x.Status);
        }

        public async Task ChangeStatus(UserChangeStatusInputDto changeDto)
        {
            string userids = string.Join<long>(",", changeDto.UserIds);
            await _userRepository.UpdateRangeAsync(u => userids.Contains(u.ID.ToString()), u => new SysUser { Status = changeDto.Status });
        }

        public async Task Delete(long Id)
        {
            if (Id <= 2)
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"不能删除初始用户"));
            }
            await _userRepository.UpdateAsync(new SysUser() { ID = Id, Status = (int)ManageStatus.Deleted }, x => x.Status);
        }

        public async Task Save(UserSaveInputDto saveDto)
        {
            var user = _mapper.Map<SysUser>(saveDto);
            if (user.ID < 1)
            {
                if (await _userRepository.ExistAsync(x => x.Account == user.Account))
                {
                    throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"用户已存在"));
                }

                user.ID = IdGenerater.GetNextId();
                user.Salt = SecurityHelper.GenerateRandomCode(5);
                user.Password = HashHelper.GetHashedString(HashType.MD5, user.Password, user.Salt);
                await _usrManager.AddUser(user);
            }
            else
            {
                await _userRepository.UpdateAsync(user,
                     x => x.Name,
                     x => x.DeptId,
                     x => x.Sex,
                     x => x.Phone,
                     x => x.Email,
                     x => x.Birthday,
                     x => x.Status);
            }
        }

        public async Task<PageModelDto<UserDto>> GetPaged(UserSearchDto searchDto)
        {
            Expression<Func<SysUser, bool>> whereCondition = x => x.Status > 0;
            if (!string.IsNullOrWhiteSpace(searchDto.Account))
            {
                whereCondition = whereCondition.And(x => x.Account.Contains(searchDto.Account));
            }

            if (!string.IsNullOrWhiteSpace(searchDto.Name))
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(searchDto.Name));
            }

            var pagedModel = await _userRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, whereCondition, x => x.ID, false);
            var result = _mapper.Map<PageModelDto<UserDto>>(pagedModel);
            if (result.Count > 0)
            {
                var deptIds = result.Data.Where(d => d.DeptId != null).Select(d => d.DeptId).Distinct().ToList();
                //var roleIds = roleIdstring.ToArray().Distinct().Where(x => x!=char.MinValue && x != ',').ToArray();

                var depts = await _deptRepository.SelectAsync(d => new { d.ID, d.FullName }, x => deptIds.Contains(x.ID));
                var roles = await _roleRepository.SelectAsync(r => new { r.ID, r.Name }, x => true);
                foreach (var user in result.Data)
                {
                    user.DeptName = depts.FirstOrDefault(x => x.ID == user.DeptId)?.FullName;
                    var roleIds = string.IsNullOrWhiteSpace(user.RoleId) 
                        ? new List<long>()
                        : user.RoleId.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x))
                        ;
                    user.RoleName = string.Join(',', roles.Where(x => roleIds.Contains(x.ID)).Select(x => x.Name));
                }
            }

            return result;
        }

        public async Task SetRole(RoleSetInputDto setDto)
        {
            if (setDto.ID < 1)
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden, "禁止修改管理员角色"));
            }
            var roleIdStr = setDto.RoleIds == null ? null : string.Join(",", setDto.RoleIds);
            await _userRepository.UpdateAsync(new SysUser() { ID = setDto.ID, RoleId = roleIdStr }, x => x.RoleId);
        }
    }
}
