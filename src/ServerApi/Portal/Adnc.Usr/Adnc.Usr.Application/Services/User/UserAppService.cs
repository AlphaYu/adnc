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
        private readonly UsrManager _usrManager;

        public UserAppService(IMapper mapper,
            IEfRepository<SysUser> userRepository,
            IDeptAppService deptAppService,
            IRoleAppService roleAppService,
            UsrManager usrManager)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _deptAppService = deptAppService;
            _roleAppService = roleAppService;
            _usrManager = usrManager;
        }

        public async Task<AppSrvResult> ChangeStatusAsync(long id, int status)
        {
            await _userRepository.UpdateAsync(new SysUser { Id = id, Status = status }, UpdatingProps<SysUser>(x => x.Status));
            return AppSrvResult();
        }

        public async Task<AppSrvResult> ChangeStatusAsync(UserChangeStatusDto input)
        {
            string userids = string.Join<long>(",", input.UserIds);
            await _userRepository.UpdateRangeAsync(u => userids.Contains(u.Id.ToString()), u => new SysUser { Status = input.Status });
            return AppSrvResult();
        }

        public async Task<AppSrvResult> DeleteAsync(long id)
        {
            await _userRepository.DeleteAsync(id);
            return AppSrvResult();
        }

        public async Task<AppSrvResult<long>> CreateAsync(UserCreationDto input)
        {
            if (await _userRepository.AnyAsync(x => x.Account == input.Account))
                return Problem(HttpStatusCode.BadRequest, "账号已经存在");

            var user = _mapper.Map<SysUser>(input);
            user.Id = IdGenerater.GetNextId();
            user.Salt = SecurityHelper.GenerateRandomCode(5);
            user.Password = HashHelper.GetHashedString(HashType.MD5, user.Password, user.Salt);
            await _userRepository.InsertAsync(user);

            return user.Id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id,UserUpdationDto input)
        {
            var user = _mapper.Map<SysUser>(input);

            user.Id = id;

            var updatingProps = UpdatingProps<SysUser>(x => x.Name,
                                                       x => x.DeptId,
                                                       x => x.Sex,
                                                       x => x.Phone,
                                                       x => x.Email,
                                                       x => x.Birthday,
                                                       x => x.Status
                                                      );
            await _userRepository.UpdateAsync(user, updatingProps);

            return AppSrvResult();
        }

        public async Task<AppSrvResult<PageModelDto<UserDto>>> GetPagedAsync(UserSearchPagedDto search)
        {
            Expression<Func<SysUser, bool>> whereCondition = x => true;
            if (search.Account.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Account.Contains(search.Account));
            }

            if (search.Name.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(search.Name));
            }

            var pagedModel = await _userRepository.PagedAsync(search.PageIndex, search.PageSize, whereCondition, x => x.Id, false);
            var pageModelDto = _mapper.Map<PageModelDto<UserDto>>(pagedModel);

            pageModelDto.XData = await _deptAppService.GetSimpleListAsync();

            if (pageModelDto.RowsCount > 0)
            {
                var deptIds = pageModelDto.Data.Where(d => d.DeptId != null).Select(d => d.DeptId).Distinct().ToList();
                var depts = (await _deptAppService.GetAllFromCacheAsync())
                            .Where(x => deptIds.Contains(x.Id))
                            .Select(d => new { d.Id, d.FullName });
                var roles = (await _roleAppService.GetAllFromCacheAsync())
                            .Select(r => new { r.Id, r.Name });

                foreach (var user in pageModelDto.Data)
                {
                    user.DeptName = depts.FirstOrDefault(x => x.Id == user.DeptId)?.FullName;
                    var roleIds = string.IsNullOrWhiteSpace(user.RoleIds)
                        ? new List<long>()
                        : user.RoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x))
                        ;
                    user.RoleNames = string.Join(',', roles.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name));
                }
            }

            return pageModelDto;
        }

        public async Task<AppSrvResult> SetRoleAsync(long id, UserSetRoleDto input)
        {
            var roleIdStr = input.RoleIds == null ? null : string.Join(",", input.RoleIds);
            await _userRepository.UpdateAsync(new SysUser() { Id = id, RoleIds = roleIdStr }, UpdatingProps<SysUser>(x => x.RoleIds));

            return AppSrvResult();
        }
    }
}
