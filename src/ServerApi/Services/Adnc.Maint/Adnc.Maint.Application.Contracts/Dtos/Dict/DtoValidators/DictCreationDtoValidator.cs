using Adnc.Core.Shared.EntityConsts.Maint;
using Adnc.Maint.Application.Contracts.Dtos;
using FluentValidation;

namespace Adnc.Maint.Application.Contracts.DtoValidators
{
    public class DictCreationDtoValidator : AbstractValidator<DictCreationDto>
    {
        public DictCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(DictConsts.Name_MaxLength);
            RuleFor(x => x.Value).MaximumLength(DictConsts.Value_MaxLength);
        }
    }
}