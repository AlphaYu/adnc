using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class MenuSaveInputDtoValidator : AbstractValidator<MenuSaveInputDto>
    {
        public MenuSaveInputDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2,16);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Code).NotEmpty().Length(2,16);
            RuleFor(x => x.Component).MaximumLength(64);
            RuleFor(x => x.Icon).MaximumLength(16);
            RuleFor(x => x.PCode).MaximumLength(16);
        }
    }
}
