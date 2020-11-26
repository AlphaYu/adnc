using FluentValidation;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.DtoValidators;

namespace Adnc.Maint.Application.DtoValidators
{
    public class CfgSaveInputDtoValidator: AbstractValidator<CfgSaveInputDto>
    {
        public CfgSaveInputDtoValidator()
        {
            RuleFor(x => x.CfgName).NotEmpty().Length(2,64);
            RuleFor(x => x.CfgValue).NotEmpty().Length(2,128);
            RuleFor(x => x.CfgDesc).MaximumLength(256);
        }
    }
}
