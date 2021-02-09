using FluentValidation;
using Adnc.Maint.Application.Dtos;

namespace Adnc.Maint.Application.DtoValidators
{
    public class DictCreationDtoValidator : AbstractValidator<DictCreationDto>
    {
        public DictCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(16);
            RuleFor(x => x.Value).MaximumLength(64);
        }
    }
}
