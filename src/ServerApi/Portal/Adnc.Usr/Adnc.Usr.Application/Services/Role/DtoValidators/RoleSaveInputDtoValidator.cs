using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class RoleSaveInputDtoValidator : AbstractValidator<RoleSaveInputDto>
    {
        public RoleSaveInputDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 32);
            RuleFor(x => x.Tips).NotEmpty().Length(2, 64);
        }
    }
}
