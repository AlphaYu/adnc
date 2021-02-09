using FluentValidation;
using Adnc.Usr.Application.Dtos;
using Adnc.Infr.Common.Extensions;

namespace Adnc.Usr.Application.DtoValidators
{
    public class MenuCreationDtoValidator : AbstractValidator<MenuCreationDto>
    {
        public MenuCreationDtoValidator()
        {
            RuleFor(x => x.Code).NotEmpty().Length(2, 16);
            RuleFor(x => x.PCode).MaximumLength(16).NotEqual(x => x.Code).When(x => x.PCode.IsNotNullOrWhiteSpace());
            RuleFor(x => x.Name).NotEmpty().Length(2, 16);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Component).MaximumLength(64);
            RuleFor(x => x.Icon).MaximumLength(16);
        }
    }
}
