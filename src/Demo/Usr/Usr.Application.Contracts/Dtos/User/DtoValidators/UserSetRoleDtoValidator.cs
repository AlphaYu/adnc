namespace Adnc.Demo.Usr.Application.Contracts.DtoValidators
{
    public class UserSetRoleDtoValidator : AbstractValidator<UserSetRoleDto>
    {
        public UserSetRoleDtoValidator()
        {
            //RuleFor(x => x.Id).Equal(1600000000010).WithMessage("禁止修改系统管理员角色");
            RuleFor(x => x.RoleIds).NotEmpty();
            //RuleFor(x => x.RoleIds).ForEach(x => x.LessThan(2).WithMessage(""));
        }
    }
}