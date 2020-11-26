using FluentValidation;
using Adnc.Maint.Application.Dtos;

namespace Adnc.Maint.Application.DtoValidators
{
    public class DictSaveInputDtoValidator : AbstractValidator<DictSaveInputDto>
    {
        public DictSaveInputDtoValidator()
        {
            RuleFor(x => x.DictName).NotEmpty().MaximumLength(16);
            RuleFor(x => x.Tips).MaximumLength(64);
        }
    }
}
