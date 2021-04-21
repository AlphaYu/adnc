using FluentValidation;
using Adnc.Maint.Application.Contracts.Dtos;

namespace Adnc.Maint.Application.Contracts.DtoValidators
{
    public class CfgCreationDtoValidator: AbstractValidator<CfgCreationDto>
    {
        public CfgCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2,64);
            RuleFor(x => x.Value).NotEmpty().Length(2,128);
            RuleFor(x => x.Description).MaximumLength(256);
        }
    }
}
