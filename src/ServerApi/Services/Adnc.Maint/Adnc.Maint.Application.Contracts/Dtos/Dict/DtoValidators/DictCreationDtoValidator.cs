using FluentValidation;
using Adnc.Maint.Application.Contracts.Dtos;

namespace Adnc.Maint.Application.Contracts.DtoValidators
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
