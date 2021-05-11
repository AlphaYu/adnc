using FluentValidation;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Core.Shared.EntityConsts.Usr;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class RoleCreationDtoValidator : AbstractValidator<RoleCreationDto>
    {
        public RoleCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, RoleConsts.Name_MaxLength);
            RuleFor(x => x.Tips).NotEmpty().Length(2, RoleConsts.Tips_MaxLength);
        }
    }
}
