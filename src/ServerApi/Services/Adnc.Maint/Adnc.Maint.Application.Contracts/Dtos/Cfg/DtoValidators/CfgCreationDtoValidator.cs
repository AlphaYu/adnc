using Adnc.Core.Shared.EntityConsts.Maint;
using Adnc.Maint.Application.Contracts.Dtos;
using FluentValidation;

namespace Adnc.Maint.Application.Contracts.DtoValidators
{
    public class CfgCreationDtoValidator : AbstractValidator<CfgCreationDto>
    {
        public CfgCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, CfgConsts.Name_MaxLength);
            RuleFor(x => x.Value).NotEmpty().Length(2, CfgConsts.Value_MaxLength);
            RuleFor(x => x.Description).MaximumLength(CfgConsts.Description_MaxLength);
        }
    }
}