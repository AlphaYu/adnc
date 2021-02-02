using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class RoleSetInputDtoValidator : AbstractValidator<RoleSetInputDto>
    {
        public RoleSetInputDtoValidator()
        {
            //RuleFor(x => x.Id).Equal(1600000000010).WithMessage("禁止修改系统管理员角色");
            RuleFor(x => x.RoleIds).NotEmpty();
            //RuleFor(x => x.RoleIds).ForEach(x => x.LessThan(2).WithMessage(""));
        }
    }
}